using System;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;

using CodeJam.Services;

using JRO;

namespace Rsdn.Janus.Jet
{
	/// <summary>
	/// Обработчик команд модуля Jet.
	/// </summary>
	[CommandTarget]
	internal sealed class JetCommandTarget : CommandTarget
	{
		public JetCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Jet.CompactDb")]
		public void ExecuteCompactDb(ICommandContext context)
		{
			ProgressWorker.Run(
				context,
				false,
				progressVisualizer =>
				{
					progressVisualizer.SetProgressText(Resources.CompactDbProgressText);
					
					var janusDatabaseManager = context.GetRequiredService<IJanusDatabaseManager>();
					
					using (janusDatabaseManager.GetLock().GetWriterLock())
					{
						var connectionString = janusDatabaseManager.GetCurrentConnectionString();
						var csb = new JetConnectionStringBuilder(connectionString);
						var dbFile = csb.DataSource;
						var tmpFile = dbFile + ".temp";
						var backupFile = dbFile + ".bak";

						csb.DataSource = tmpFile;

						if (File.Exists(tmpFile))
							File.Delete(tmpFile);

						OleDbConnection.ReleaseObjectPool();

						// ReSharper disable once SuspiciousTypeConversion.Global
						var engine = (IJetEngine) new JetEngineClass();
						try
						{
							engine.CompactDatabase(connectionString, csb.ConnectionString);
						}
						finally
						{
							Marshal.ReleaseComObject(engine);
						}

						if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
						{
							// Unsafe replace
							//
							File.Move(tmpFile, dbFile);
							File.Delete(tmpFile);
						}
						else
						{
							// Safe replace
							//
							File.Replace(tmpFile, dbFile, backupFile);
							File.Delete(backupFile);
						}
					}
				});
		}

		[CommandStatusGetter("Janus.Jet.CompactDb")]
		public CommandStatus QueryCompactDbStatus(ICommandContext context)
		{
			var janusDatabaseManager = context.GetService<IJanusDatabaseManager>();

			return janusDatabaseManager != null
				&& janusDatabaseManager.GetCurrentDriverName() == JetDriver.DriverName
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}
	}
}