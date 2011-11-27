using System;
using System.Diagnostics;

using System.IO;

namespace HgVersion
{
	class Program
	{
		private static string GetVersionFromHg(string repoDir)
		{
			var p =
				new Process
				{
					StartInfo =
						{
							UseShellExecute = false,
							RedirectStandardOutput = true,
							RedirectStandardError = true,
							CreateNoWindow = true,
							WorkingDirectory = repoDir,
							FileName = "hg",
							Arguments = "log -r tip --template {latesttag}.{latesttagdistance}"
						}
				};
			p.Start();

			var output = p.StandardOutput.ReadToEnd().Trim();

			//string error = p.StandardError.ReadToEnd().Trim();

			p.WaitForExit();

			return output;
		}

		static int Main(string[] args)
		{
			if (args.Length != 3)
			{
				Console.WriteLine("Usage: HgVersion.exe <path_to_repo> <template_path> <output_path>");
				return -1;
			}

			var ver = GetVersionFromHg(args[0]).Split('.');

			var major = ver[0];
			var minor = ver.Length > 1 ? ver[1] : "0";
			var build = ver.Length > 2 ? ver[2] : "0";
			var rev = ver.Length > 3 ? ver[3] : "0";

			var output =
				File
					.ReadAllText(args[1])
					.Replace("$VER_MAJOR$", major)
					.Replace("$VER_MINOR$", minor)
					.Replace("$VER_BUILD$", build)
					.Replace("$VER_REVISION$", rev);

			File.WriteAllText(args[2], output);

			return 0;
		}
	}
}
