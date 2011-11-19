using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Rsdn.Janus
{
	[JanusDBDriver(MssqlDriver.DriverName)]
	internal class MssqlDriver: IDBDriver
	{
		public const string DriverName = "Mssql";

		#region IDBDriver Members

		public DbEngineType DbEngineType
		{
			get { return DbEngineType.MsSqlDB; }
		}

		public bool CheckConnectionString(string constr)
		{
			try
			{
				var csbCheck = new SqlConnectionStringBuilder(constr)
					{
						ConnectTimeout = Config.Instance.ConnectTimeout
					};

				using (var con = new SqlConnection(csbCheck.ConnectionString))
					con.Open();
			}
			catch
			{
				return false;
			}
			return true;
		}

		public void CreateDatabase(string connectionString)
		{
			string dbName;

			using (var con = ConnectToMaster(connectionString, out dbName))
			using (var cmd = con.CreateCommand())
			{
				con.Open();
				cmd.CommandText = string.Format("CREATE DATABASE {0}", dbName);
				cmd.ExecuteNonQuery();
			}
		}

		public IDbscProvider CreateSchemaProvider()
		{
			return new DbscProviderMssql();
		}

		public DbConnectionStringBuilder CreateConnectionString()
		{
			return new SqlConnectionStringBuilder();
		}

		public DbConnectionStringBuilder CreateConnectionString(string constr)
		{
			return new SqlConnectionStringBuilder(constr);
		}

		public IDBConfigControl CreateConfigControl()
		{
			return new MssqlConfigControl();
		}

		/// <summary>
		/// Поддерживаемые драйвером действия над бд.
		/// </summary>
		public DbTask SupportedTasks
		{
			get { return DbTask.Compact; }
		}

		/// <summary>
		/// Выполнить действие над бд.
		/// </summary>
		public void ExecTask(DbTask task, string connectionString)
		{
			switch (task)
			{
				case DbTask.Compact:
					CompactDb(connectionString);
					break;
				default:
					throw new ArgumentOutOfRangeException("task");
			}
		}

		#endregion

		private static SqlConnection ConnectToMaster(string connectionString, out string dbName)
		{
			var csb = new SqlConnectionStringBuilder(connectionString);

			dbName = csb.InitialCatalog;
			csb.InitialCatalog = string.Empty;

			return new SqlConnection(csb.ConnectionString);
		}

		private static void CompactDb(string connectionString)
		{
			// Ensure there is no cached connections to the DB
			//
			SqlConnection.ClearAllPools();

			string dbName;
			using (var con = ConnectToMaster(connectionString, out dbName))
			using (var cmd = con.CreateCommand())
			{
				con.Open();

				// This is a REALLY long operation
				//
				cmd.CommandTimeout = Int32.MaxValue;

				// Set the database in single user mode.
				//
				cmd.CommandText = "EXEC sp_dboption '" + dbName + "', 'single user', 'True'";
				cmd.ExecuteNonQuery();

				// Shrink the database size to the size + 10 percent of free space.
				// The truncateonly attribute releases the shrunken space to
				// the operating system.
				//
				cmd.CommandText = "DBCC ShrinkDatabase(" + dbName + ", 10, TRUNCATEONLY)";
				cmd.ExecuteNonQuery();

				// Checks the integrity of the db, and repairs some issues without
				// data loss. This will rebuild your indexes.
				//
				cmd.CommandText = "DBCC CheckDB(" + dbName + ", REPAIR_REBUILD)";
				cmd.ExecuteNonQuery();

				// Sets the db back to full access.
				//
				cmd.CommandText = "EXEC sp_dboption '" + dbName + "', 'single user', 'False'";
				cmd.ExecuteNonQuery();
			}
		}
	}
}
