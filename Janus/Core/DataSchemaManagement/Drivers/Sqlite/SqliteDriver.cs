using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	[JanusDBDriver(SqliteDriver.DriverName)]
	internal class SqliteDriver: IDBDriver
	{
		public const string DriverName = "Sqlite";

		#region IDBDriver Members

		public DbEngineType DbEngineType
		{
			get { return DbEngineType.SQLiteDB; }
		}

		public bool CheckConnectionString(string constr)
		{
			try
			{
				var csbCheck = new SQLiteConnectionStringBuilder(constr);
				using (var con = new SQLiteConnection(csbCheck.ConnectionString))
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
			// SQLite молча пересоздаст файл если такой уже есть.
			//
			var csb = new SQLiteConnectionStringBuilder(constr);
			if (!File.Exists(csb.DataSource) || MessageBox.Show("Файл '"
					+ Path.GetFileName(csb.DataSource) + "' уже сужествует. Заменить?",
				"SQLite", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				SQLiteConnection.CreateFile(csb.DataSource);
			}
		}

		public IDbscProvider CreateSchemaProvider()
		{
			return new DbscProviderSqlite();
		}

		public DbConnectionStringBuilder CreateConnectionString()
		{
			return CreateConnectionString(null);
		}

		public DbConnectionStringBuilder CreateConnectionString(string constr)
		{
			var csb = new SQLiteConnectionStringBuilder(constr) {LegacyFormat = false};
			return csb;
		}

		public IDBConfigControl CreateConfigControl()
		{
			return new SqliteConfigControl();
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
			using (var con = new SQLiteConnection(connectionString))
			using (var cmd = con.CreateCommand())
			{
				con.Open();

				// This is a REALLY long operation
				//
				cmd.CommandTimeout = Int32.MaxValue;

				// Clean up the backend database file
				//
				cmd.CommandText = "VACUUM";
				cmd.ExecuteNonQuery();
			}
		}
	}
}
