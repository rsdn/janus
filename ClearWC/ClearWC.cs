using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Rsdn.Utils;
using System.Collections;

namespace ClearWC
{
	internal class ClearWC
	{
		private static void Main(string[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("Usage: ClearWC.exe PathToSvnWorkingCopy");
				return;
			}
			if (!Directory.Exists(args[0]))
			{
				Console.WriteLine("Directory {0} not exists", args[0]);
			}
			Clear(args[0]);
		}

		private static void Clear(string wcDir)
		{
			Dictionary<string, object> wcFiles = new Dictionary<string, object>();
			Dictionary<string, object> wcDirs = new Dictionary<string, object>();
			SvnWorkingDirectory wc = new SvnWorkingDirectory(wcDir);
			wc.Scan(
				delegate(string path, IDictionary props)
				{
					string ffn = Path.Combine(wcDir, path);
					wcFiles[ffn] = null;
				},
				delegate(string path, IDictionary props)
				{
					string fdn = Path.Combine(wcDir, path);
					wcDirs[fdn] = null;
				});

			ClearDir(wcDir, wcFiles, wcDirs);

			Console.WriteLine("Done.");
		}

		private static void ClearDir(string dir,
			IDictionary<string, object> wcFiles,
			IDictionary<string, object> wcDirs)
		{
			foreach (string cd in Directory.GetDirectories(dir))
			{
				if (Path.GetFileName(cd) == SvnWorkingDirectory.SvnDirName)
					continue;
				if (!wcDirs.ContainsKey(cd))
				{
					Console.WriteLine("Delete directory '{0}'", cd);
					try
					{
						Directory.Delete(cd, true);
					}
					catch (Exception e)
					{
						Console.WriteLine("  Error - {0}", e.Message);
					}
				}
				else
					ClearDir(cd, wcFiles, wcDirs);
			}
			foreach (string fn in Directory.GetFiles(dir))
				if (!wcFiles.ContainsKey(fn))
				{
					Console.WriteLine("Delete file '{0}'", fn);
					try
					{
						File.Delete(fn);
					}
					catch (Exception e)
					{
						Console.WriteLine("  Error - {0}", e.Message);
					}
				}
		}
	}
}
