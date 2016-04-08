using System;
using System.Data.SqlClient;

using CodeJam.Services;

namespace Rsdn.Janus.Mssql
{
	/// <summary>
	/// Обработчик команд модуля Mssql.
	/// </summary>
	[CommandTarget]
	internal sealed class MssqlCommandTarget : CommandTarget
	{
		public MssqlCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandExecutor("Janus.Mssql.CompactDb")]
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

						// Ensure there is no cached connections to the DB
						//
						SqlConnection.ClearAllPools();

						string dbName;
						using (var con = MssqlSchemaDriver.ConnectToMaster(connectionString, out dbName))
						using (var cmd = con.CreateCommand())
						{
							con.Open();

							// This is a REALLY long operation
							//
							cmd.CommandTimeout = Int32.MaxValue;

							// Set the database in single user mode.
							//
							cmd.CommandText = @"EXEC sp_dboption '" + dbName + @"', 'single user', 'True'";
							cmd.ExecuteNonQuery();

							// Shrink the database size to the size + 10 percent of free space.
							// The truncateonly attribute releases the shrunken space to
							// the operating system.
							//
							cmd.CommandText = @"DBCC ShrinkDatabase(" + dbName + @", 10, TRUNCATEONLY)";
							cmd.ExecuteNonQuery();

							// Checks the integrity of the db, and repairs some issues without
							// data loss. This will rebuild your indexes.
							//
							cmd.CommandText = @"DBCC CheckDB(" + dbName + @", REPAIR_REBUILD)";
							cmd.ExecuteNonQuery();

							// Sets the db back to full access.
							//
							cmd.CommandText = @"EXEC sp_dboption '" + dbName + @"', 'single user', 'False'";
							cmd.ExecuteNonQuery();
						}
					}
				});
		}

		[CommandStatusGetter("Janus.Mssql.CompactDb")]
		public CommandStatus QueryCompactDbStatus(ICommandContext context)
		{
			var janusDatabaseManager = context.GetService<IJanusDatabaseManager>();

			return janusDatabaseManager != null
				   && janusDatabaseManager.GetCurrentDriverName() == MssqlDriver.DriverName
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}
	}
}