using System;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace SvnRevision
{
	/// <summary>
	///  Замена SubWCRev.exe, см. сообщение
	///  http://gzip.rsdn.ru/forum/Message.aspx?mid=871939&only=1
	///  
	///  Идея: просканировать скрытые папки .svn в рабочей копии репозитория, воспользоваться
	///  служебным файлом entries в формате XML и вытащить из него атрибут revision и выдать
	///  его максимальное значение. Затем просканировать шаблон и заменить вхождения $WCREV$ на
	///  номер ревизии, и сохранить полученное, только если были изменения.
	/// </summary>
	internal class Program
	{
		private const string _argumentInfo =
			"Need three arguments: <path to working folder> <path to template> <path to dest>.";

		private const string _programInfo =
			"\n\rSvnRevision read the Subversion status of all files in a working folder. Then the highest revision number found is used to replace all occurences of $WCREV$ in template and save it, if made changes in revision number\n\r";

		private const string _replaceMark = "$WCREV$";

		private const string _separateLine = "----------------------------------------------";

		private const string _svnAspDotNetHack              = "SVN_ASP_DOT_NET_HACK";
		private const string _svnDirectoryNameAspDotNetHack = "_svn";
		private const string _svnDirectoryNameCommon        = ".svn";

		private const string _svnEntriesFileName            = "entries";
		private const string _svnFormatFileName             = "format";
		private const string _xpathRevision                 = @"ns:wc-entries/ns:entry/@revision";

	    private const string _svnDBFileName                 = "wc.db";
		
		private static readonly string _svnDirectoryName =
			Environment.GetEnvironmentVariable(_svnAspDotNetHack) != null
									? _svnDirectoryNameAspDotNetHack
									: _svnDirectoryNameCommon;

		[STAThread]
		private static int Main(string[] args)
		{
			try
			{
				Console.WriteLine(_programInfo);

				if (args.Length != 3)
					throw new ArgumentException(_argumentInfo);

				var workingFolder   = args[0];
				var templatePath    = args[1];
				var destinationPath = args[2];


    	        if (!Directory.Exists(workingFolder))
			        throw new DirectoryNotFoundException();

			    if (!File.Exists(templatePath))
			        throw new FileNotFoundException(string.Empty, templatePath);

			    var revision = FindRevision(new DirectoryInfo(workingFolder), 0);
			    Console.WriteLine("Current Revision is {0}", revision);

			    ProcessTemplate(templatePath, destinationPath, revision);

			    return 0;
			}
			catch (Exception e)
			{
				Console.WriteLine(_separateLine);
				Console.WriteLine(e);
				Console.WriteLine(_separateLine);

				return 1; // Код ошибки для студии
			}
		}

		private static void ProcessTemplate(string templatePath, 
			string destinationPath, int revision)
		{
			var encoding  = Encoding.Default;
			var destination = string.Empty;

			string template;

			using (var templateStream =
				new StreamReader(templatePath, encoding, true))
			{
				template = templateStream.ReadToEnd();
				encoding = templateStream.CurrentEncoding;
			}

			template = template.Replace(_replaceMark,
				revision.ToString(CultureInfo.InvariantCulture));

			if (File.Exists(destinationPath))
				destination = File.ReadAllText(destinationPath, encoding);


			if (string.Equals(template, destination, StringComparison.InvariantCulture))
				return;

			File.WriteAllText(destinationPath, template, encoding);
		}

		private static int FindRevision(DirectoryInfo directoryInfo, int maxRevision)
		{
			RevisionGetter revisionGetter = null;

			foreach (var subDir in directoryInfo.GetDirectories())
			{
				var revision = 0;

				if (string.Equals(subDir.Name, _svnDirectoryName,
					StringComparison.InvariantCultureIgnoreCase))
				{
					if ((revisionGetter ?? (revisionGetter = GetRevisionGetter(subDir))) != null)
						revision = revisionGetter(subDir, maxRevision);
				}
				else
					revision = FindRevision(subDir, maxRevision);

				if (revision > maxRevision)
					maxRevision = revision;
			}

			return maxRevision;
		}

		private static RevisionGetter GetRevisionGetter(DirectoryInfo directoryInfo)
		{
			if (directoryInfo == null)
				throw new ArgumentNullException("directoryInfo");

			var filePath = Path.Combine(directoryInfo.FullName, _svnFormatFileName);

			if (File.Exists(filePath))
			{
				var version = File.ReadAllText(filePath, Encoding.ASCII);

				switch (version.TrimEnd())
				{
					case "4": return GetRevisionVer4;
					case "8":
					case "9": return GetRevisionVer8;
                    case "12": return GetRevisionVer12;
				}
			}
			else
			{
				// C 10 версии файл 'format' отсутсвует.
				// Пробуем прочитать версию в первой строке файла 'entries'
				filePath = Path.Combine(directoryInfo.FullName, _svnEntriesFileName);
				if (File.Exists(filePath))
				{
					using (var sr = new StreamReader(filePath))
					{
						var line = sr.ReadLine();
						int dbVer;
						if (line != null && Int32.TryParse(line, out dbVer))
						{
							if (dbVer == 10)
								return GetRevisionVer8;
						}
					}
				}
			}

			return delegate { return 0; };
		}

		private static int GetRevisionVer4(DirectoryInfo dir, int maxRevision)
		{
			var filePath = Path.Combine(dir.FullName, _svnEntriesFileName);

			if (!File.Exists(filePath))
				return maxRevision;

			using (var reader = new StreamReader(filePath))
			{
				var  doc = new XPathDocument(reader);
				var nav = doc.CreateNavigator();

			    if (nav.NameTable != null)
			    {
			        var manager = new XmlNamespaceManager(nav.NameTable);
			        manager.AddNamespace("ns", "svn:");

			        var expr = nav.Compile(_xpathRevision);
			        expr.SetContext(manager);

			        var iterator = nav.Select(expr);

			        while (iterator.MoveNext())
			        {
			            try
			            {
			                if (iterator.Current != null)
			                {
			                    var revision = int.Parse(iterator.Current.Value,
			                                             NumberStyles.Integer, CultureInfo.InvariantCulture);

			                    if (revision > maxRevision)
			                        maxRevision = revision;
			                }
			            }
			            catch (FormatException)
			            {
			            }
			        }
			    }
			}

			return maxRevision;
		}

		private static int GetRevisionVer8(DirectoryInfo dir, int maxRevision)
		{
			var filePath = Path.Combine(dir.FullName, _svnEntriesFileName);

			if (!File.Exists(filePath))
				return maxRevision;

			try
			{
				using (var sr = new StreamReader(filePath))
				{
					var lineCounter = 0;

					string line;
					while ((line = sr.ReadLine()) != null)
					{
						if (lineCounter == 3)
						{
							var revision = int.Parse(line, 
								NumberStyles.Integer, CultureInfo.InvariantCulture);

							if (revision > maxRevision)
								maxRevision = revision;

							break;
						}

						lineCounter++;
					}
				}
			}
			catch (FormatException)
			{
			}

			return maxRevision;
		}

		private static int GetRevisionVer12(DirectoryInfo dir, int maxRevision)
		{
			string filePath = Path.Combine(dir.FullName, _svnDBFileName);

			if (!File.Exists(filePath))
				return maxRevision;

			string connectionString = string.Format("Data Source={0};Read Only=True", filePath);

			using (var conn = new SQLiteConnection { ConnectionString = connectionString })
			{
				conn.Open();
				using (SQLiteCommand cmd = conn.CreateCommand())
				{
					cmd.CommandText = "SELECT MAX(revision) FROM nodes;";
					maxRevision = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);
				}
			}

			return maxRevision;
		}

		#region Nested type: RevisionGetter

		private delegate int RevisionGetter(DirectoryInfo dir, int maxRevision);

		#endregion
	}
}