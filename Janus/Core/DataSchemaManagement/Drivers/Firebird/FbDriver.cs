using System.Data.Common;
using FirebirdSql.Data.FirebirdClient;

namespace Rsdn.Janus
{
	[JanusDBDriver(FBDriver.DriverName)]
	internal class FBDriver : IDBDriver
	{
		public const string DriverName = "Fb";

		#region IDBDriver Members

		public DbEngineType DbEngineType
		{
			get { return DbEngineType.FireBirdDB; }
		}

		public bool CheckConnectionString(string constr)
		{
			try
			{
				var csbCheck = new FbConnectionStringBuilder(constr) {Pooling = false};

				using (var con = new FbConnection(csbCheck.ConnectionString))
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
			var csb = new FbConnectionStringBuilder(constr) {Pooling = false};

			FbConnection.CreateDatabase(csb.ConnectionString, 16384, false, true);

			using (var con = new FbConnection(csb.ConnectionString))
			using (var cmd = con.CreateCommand())
			{
				con.Open();

				#region bug drug block
				//cmd.CommandText = @"CREATE TABLE crdb (tid INTEGER, name CHAR(120));";
				//cmd.ExecuteScalar();
				//cmd.CommandText = @"DROP TABLE crdb;";
				//cmd.ExecuteScalar();
				#endregion

				#region init actions: register udf functions

				cmd.CommandText = @"
					DECLARE EXTERNAL FUNCTION strlen 
						CSTRING(32767)
						RETURNS INTEGER BY VALUE
						ENTRY_POINT 'IB_UDF_strlen' MODULE_NAME 'ib_udf';";
				cmd.ExecuteScalar();

				#endregion
			}
		}

		public IDbscProvider CreateSchemaProvider()
		{
			return new DbscProviderFb();
		}

		public DbConnectionStringBuilder CreateConnectionString()
		{
			return new FbConnectionStringBuilder();
		}

		public DbConnectionStringBuilder CreateConnectionString(string constr)
		{
			return new FbConnectionStringBuilder(constr);
		}

		public IDBConfigControl CreateConfigControl()
		{
			return new FbConfigControl();
		}

		/// <summary>
		/// Поддерживаемые драйвером действия над бд.
		/// </summary>
		public DbTask SupportedTasks
		{
			get { return DbTask.None; }
		}

		/// <summary>
		/// Выполнить действие над бд.
		/// </summary>
		public void ExecTask(DbTask task, string connectionString)
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}
