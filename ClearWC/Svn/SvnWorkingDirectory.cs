using System.Collections;
using System.IO;

using Rsdn.Utils.SvnEntries;
using System;

namespace Rsdn.Utils
{
	public class SvnWorkingDirectory
	{
		public const string SvnDirName = ".svn";
		private const string _svnEntriesFile = "entries";
		private const string _svnPropsFileTemplate = SvnDirName + "/props/{0}.svn-work";
		private const string _svnDirPropsFile = SvnDirName + "/dir-props";

		private readonly string _path;

		public SvnWorkingDirectory(string path)
		{
			_path = path;
			if (!Directory.Exists(Path.Combine(path, SvnDirName)))
				throw new ArgumentException("Directory '" + path + ", is not SVN working copy.");
		}

		public void Scan(ProcessSvnFileCallback processFileCallback)
		{
			Scan(processFileCallback, null);
		}

		public void Scan(ProcessSvnFileCallback processFileCallback,
			ProcessSvnFileCallback processDirCallback)
		{
			RecursiveScan("", processFileCallback, processDirCallback);
		}

		private void RecursiveScan(string relDir,
			ProcessSvnFileCallback processFileCallback,
			ProcessSvnFileCallback processDirCallback)
		{
			string absDir = Path.Combine(_path, relDir);
			string  svnDir = Path.Combine(absDir, SvnDirName);
			if (!Directory.Exists(svnDir))
				return;
			WCEntries wce = WCEntries.FromFile(Path.Combine(svnDir, _svnEntriesFile));
			foreach (Entry e in wce.Entries)
			{
				if (e.Kind == EntryKind.file)
				{
					string propsFile = Path.Combine(absDir, string.Format(
						_svnPropsFileTemplate, e.Name));
					IDictionary props = new Hashtable();
					if (File.Exists(propsFile))
						FillFromHashToDisk(propsFile, props);
					processFileCallback(Path.Combine(relDir, e.Name), props);
				}
				else if ((e.Kind == EntryKind.dir) && (e.Name != ""))
				{
					string relDirName = Path.Combine(relDir, e.Name);
					if (processDirCallback != null)
					{
						IDictionary dirProps = new Hashtable();
						string dps = Path.Combine(absDir, _svnDirPropsFile);
						if (File.Exists(dps))
							FillFromHashToDisk(dps, dirProps);
						processDirCallback(relDirName, dirProps);
					}
					RecursiveScan(relDirName, processFileCallback, processDirCallback);
				}
			}
		}

		private void FillFromHashToDisk(string file, IDictionary props)
		{
			string currentKey = "";
			using (StreamReader sr = new StreamReader(file))
				while (true)
				{
					string tag = sr.ReadLine();
					if (tag == "END")
						return;
					string[] parts = tag.Split(' ');
					if (parts[0] == "K")
						currentKey = ReadCharBlock(int.Parse(parts[1]), sr);
					else if (parts[0] == "V")
						props[currentKey] = ReadCharBlock(int.Parse(parts[1]), sr);
				}
		}

		private static string ReadCharBlock(int length, StreamReader sr)
		{
			char[] buffer = new char[length];
			sr.Read(buffer, 0, length);
			return new string(buffer);
		}
	}
}