using System;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;

using ADOX;
using JRO;

namespace Rsdn.Janus
{
	/// <summary>
	/// Драйвер БД JET.
	/// </summary>
	[JanusDBDriver(JetDriver.DriverName)]
	internal class JetDriver: IDBDriver
	{
		public const string DriverName = "Jet";

		#region IDBDriver Members
		public DbEngineType DbEngineType
		{
			get { return DbEngineType.JetDB; }
		}

		public bool CheckConnectionString(string constr)
		{
			try
			{
				var csbCheck = new JetConnectionStringBuilder(constr);
				using (var con = new OleDbConnection(csbCheck.ConnectionString))
					con.Open();
			}
			catch
			{
				return false;
			}
			return true;
		}

		public void CreateDatabase(string constr)
		{
			var catalog = (Catalog)new CatalogClass();
			try
			{
				catalog.Create(constr);
			}
			finally
			{
				Marshal.ReleaseComObject(catalog);
			}
		}

		public IDbscProvider CreateSchemaProvider()
		{
			return new DbscProviderJet();
		}

		public DbConnectionStringBuilder CreateConnectionString()
		{
			return new JetConnectionStringBuilder();
		}

		public DbConnectionStringBuilder CreateConnectionString(string constr)
		{
			return new JetConnectionStringBuilder(constr);
		}

		public IDBConfigControl CreateConfigControl()
		{
			return new JetConfigControl();
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

		private static void CompactDb(string connectionString)
		{
			var csb = new JetConnectionStringBuilder(connectionString);

			var dbFile     = csb.DataSource;
			var tmpFile    = dbFile + ".temp";
			var backupFile = dbFile + ".bak";

			csb.DataSource = tmpFile;

			if (File.Exists(tmpFile))
				File.Delete(tmpFile);

			OleDbConnection.ReleaseObjectPool();

			var engine = (IJetEngine)new JetEngineClass();
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
	}
}
