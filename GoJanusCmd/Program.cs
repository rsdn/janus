using System;
using System.Windows.Forms;

using Rsdn.Janus.Framework.Ipc;

namespace GoJanusCmd
{
	internal static class Program
	{
		private const string cCommandDownload = "download";
		private const string cCommandGo = "go";

		private static int Main(string[] args)
		{
			try
			{
				if (args.Length == 0)
				{
					ShowHelp();
					return 1;
				}

				foreach (var arg in args)
					if (arg.StartsWith(cCommandGo))
					{
						var topicId = int.Parse(arg.Substring(cCommandGo.Length));
						new SendCommand().Go(topicId);
					}
					else if (arg.StartsWith(cCommandDownload))
					{
						var topicId = int.Parse(arg.Substring(cCommandDownload.Length));
						new SendCommand().Download(topicId);
					}
					else
						return 1;
			}
			catch (NamedPipeIOException)
			{
				return 2;
			}
			catch (Exception)
			{
				return 3;
			}
			return 0;
		}

		private static void ShowHelp()
		{
			var help =
				@"Command line tool for send commands to Janus.
Format:
DoJanusCmd.exe <cmd><topicID> [<cmd><topicID> ...]
where cmd:
	go - open <topicID> in Janus
	download - download <topicID> on next synchronization.

Return:
	0 - successful
	1 - incorrect command
	2 - janus not running
	3 - other error

Example:
	DoJanusCmd.exe download12585 go556565
";
			MessageBox.Show(help, "DoJanusCmd");
		}
	}
}