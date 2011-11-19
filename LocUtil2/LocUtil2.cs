using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Microsoft.CSharp;

using Rsdn.LocUtil.Helper;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Стартовый класс.
	/// </summary>
	internal static class LocUtil2
	{
		private const string msgBadArgCount = "ERROR: program arguments should be 4";
		private const string msgResourceFileNotFound = "ERROR: resource file not found";
		private const string msgOutputDirNotFound = "ERROR: output directory not found";
		private const string msgReadOnlyError = "ERROR: output file exist and read-only";
		private const string msgFileUpToDate = "INFO: nothing generated, file up-to-date";

		[STAThread]
		private static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Application.EnableVisualStyles();
				using (var mf = new MainForm())
				{
					var cont = new RootAppContainer(mf);
					cont.Add(mf);
					Application.Run(mf);
				}
			}
			else
			{
				if (args.Length != 4)
				{
					Console.WriteLine(msgBadArgCount);
					return;
				}
				if (!File.Exists(args[0]))
				{
					Console.WriteLine(msgResourceFileNotFound);
					return;
				}
				if (!Directory.Exists(args[3]))
				{
					Console.WriteLine(msgOutputDirNotFound);
					return;
				}

				Console.Write("Generate resource helper ... ");
				CultureInfo[] loadedCultures;
				DateTime lmt;
				var rc = Loader.Load(args[0], out loadedCultures,
					out lmt);
				var outFile = Path.Combine(args[3], rc.TreeName + ".cs");
				Console.WriteLine(outFile);
				if (File.Exists(outFile) && lmt <= File.GetLastWriteTime(outFile))
				{
					Console.WriteLine(msgFileUpToDate);
					return;
				}
				if (File.Exists(outFile) && (File.GetAttributes(outFile) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					Console.WriteLine(msgReadOnlyError);
					return;
				}

				bool isInternal;
				if (args[2] == "public")
					isInternal = false;
				else if (args[2] == "internal")
					isInternal = true;
				else
					throw new ArgumentException("Invalid class modifier argument");

				var ccu = HelperGenerator.Generate(rc,
					args[1], isInternal);
				var cscp = new CSharpCodeProvider();
				var cgo = new CodeGeneratorOptions {BracingStyle = "C"};
				using (var sw = new StreamWriter(
						Path.Combine(args[3], rc.TreeName + ".cs")))
					cscp.GenerateCodeFromCompileUnit(ccu, sw, cgo);
			}
		}
	}
}
