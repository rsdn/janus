using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using Rsdn.Janus.Framework.Imaging;

namespace ImageUtil
{
	static class Program
	{
		private const string _usage = "ERROR: usage <source dir> <destination resource file><image name prefix>";
		private const string _noSourceDir = "ERROR: {0} folder does not exists";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
			}
			else if(args.Length != 2 && args.Length != 3)
			{
				Console.WriteLine(_usage);
			}
			else
			{
				var sourceDirectory = args[0];
				if(!Directory.Exists(sourceDirectory))
				{
					Console.WriteLine(string.Format(_noSourceDir, sourceDirectory));
					return;
				}

				Console.WriteLine("Collecting images ...");
				var images = Utilities.CollectImages(
					sourceDirectory,
					false,
					args.Length == 3 ? args[2] : string.Empty);

				var destinationFile = args[1];

				Console.WriteLine("Generating images resource file {0} ...", destinationFile);
				try
				{
					if (destinationFile.ToLower().Contains(".resx"))
						using (var rw = new ResXResourceWriter(destinationFile))
							WriteResourceFile(rw, images);
					else
						using (var rw = new ResourceWriter(destinationFile))
							WriteResourceFile(rw, images);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}

				Console.WriteLine("{0} images converted.", images.Count);

				foreach (var image in images.Values)
					image.Dispose();
				images.Clear();
			}
		}

		private static void WriteResourceFile(IResourceWriter resources, Dictionary<string, Image> images)
		{
			foreach (var kvp in images)
				resources.AddResource(kvp.Key, kvp.Value);
		}
	}
}