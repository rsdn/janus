using System;
using System.Data.SQLite;

using CodeJam.Services;

namespace Rsdn.Janus.Sqlite
{
	/// <summary>
	/// Обработчик команд модуля Sqlite.
	/// </summary>
	[CommandTarget]
	internal sealed class SqliteCommandTarget : CommandTarget
	{
		public SqliteCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		private const string _compactDbCmd = "Janus.Sqlite.CompactDb";

		[CommandExecutor(_compactDbCmd)]
		public void ExecuteCompactDb(ICommandContext context)
		{
			ProgressWorker.Run(
				context,
				false,
				progressVisualizer =>
					{
						progressVisualizer.SetProgressText(Resources.CompactDbProgressText);

						var janusDatabaseManager = context.GetRequiredService<IJanusDatabaseManager>();

						using(janusDatabaseManager.GetLock().GetWriterLock())
						using (var con = new SQLiteConnection(janusDatabaseManager.GetCurrentConnectionString()))
						using (var cmd = con.CreateCommand())
						{
							con.Open();

							// This is a REALLY long operation
							cmd.CommandTimeout = Int32.MaxValue;

							// Clean up the backend database file
							cmd.CommandText = @"pragma page_size=" + SqliteSchemaDriver.PageSize + @"; VACUUM; ANALYZE;";
							cmd.ExecuteNonQuery();
						}
					});
		}

		private const string _updateStatsCmd = "Janus.Sqlite.UpdateStatistics";

		[CommandExecutor(_updateStatsCmd)]
		public void ExecuteUpdateStatistics(ICommandContext context)
		{
			var janusDatabaseManager = context.GetRequiredService<IJanusDatabaseManager>();

			using (janusDatabaseManager.GetLock().GetWriterLock())
			using (var con = new SQLiteConnection(janusDatabaseManager.GetCurrentConnectionString()))
			using (var cmd = con.CreateCommand())
			{
				con.Open();

				// This is a REALLY long operation
				cmd.CommandTimeout = Int32.MaxValue;

				// Clean up the backend database file
				cmd.CommandText = "ANALYZE";
				cmd.ExecuteNonQuery();
			}
		}

		[CommandStatusGetter(_compactDbCmd)]
		[CommandStatusGetter(_updateStatsCmd)]
		public CommandStatus QueryStatus(ICommandContext context)
		{
			var janusDatabaseManager = context.GetService<IJanusDatabaseManager>();

			return
				janusDatabaseManager != null
						&& janusDatabaseManager.GetCurrentDriverName() == SqliteDriver.DriverName
					? CommandStatus.Normal
					: CommandStatus.Unavailable;
		}
	}
}