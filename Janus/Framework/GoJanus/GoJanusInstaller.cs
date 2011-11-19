using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Инсталлятор плагина GoJanus.
	/// </summary>
	[RunInstaller(true)]
	public class GoJanusInstaller : Installer
	{
		private const string _pluginPath = "GoJanus/GoJanus.dll";

		private string FullPluginPath
		{
			get
			{
				return Path.Combine(
					Path.GetDirectoryName(GetType().Assembly.Location), _pluginPath);
			}
		}

		private void RegisterGoJanus()
		{
			Process.Start("regsvr32.exe", "/s " + FullPluginPath);
			Console.WriteLine("GoJanus plugin installed");
		}

		private void UnregisterGoJanus()
		{
			Process.Start("regsvr32.exe", "/s /u " + FullPluginPath);
			Console.WriteLine("GoJanus plugin uninstalled");
		}

		public override void Install(IDictionary stateSaver)
		{
			RegisterGoJanus();
			base.Install(stateSaver);
		}

		public override void Uninstall(IDictionary savedState)
		{
			UnregisterGoJanus();
			base.Uninstall(savedState);
		}

		public override void Rollback(IDictionary savedState)
		{
			UnregisterGoJanus();
			base.Rollback(savedState);
		}
	}
}