using System;
using System.Data.Common;
using System.Data.SqlClient;

using LinqToDB.DataProvider;
using LinqToDB.DataProvider.SqlServer;

namespace Rsdn.Janus.Mssql
{
	[JanusDBDriver(
		DriverName,
		"Rsdn.Janus.Mssql.Resources",
		"DriverDisplayName",
		"DriverDescription")]
	internal class MssqlDriver : IDBDriver
	{
		public const string DriverName = "MSSql";
		private readonly MssqlSqlFormatter _sqlFormatter = new MssqlSqlFormatter();

		public MssqlDriver(IServiceProvider provider)
		{
		}

		#region IDBDriver Members
		public bool CheckConnectionString(string constr)
		{
			try
			{
				var csbCheck = new SqlConnectionStringBuilder(constr);

				using (var con = new SqlConnection(csbCheck.ConnectionString))
					con.Open();
			}
			catch
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Получить драйвер схемы.
		/// </summary>
		/// <returns></returns>
		public IDBSchemaDriver CreateSchemaDriver()
		{
			return new MssqlSchemaDriver();
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
		/// Создать провайдер для BLToolkit.
		/// </summary>
		public IDataProvider CreateDataProvider()
		{
			return new SqlServerDataProvider("mssql", LinqToDB.DataProvider.SqlServer.SqlServerVersion.v2005);
		}

		/// <summary>
		/// Обработать запрос перед выполнением.
		/// </summary>
		public string PreprocessQueryText(string text)
		{
			return text;
		}

		/// <summary>
		/// Ссылка на форматтер SQL.
		/// </summary>
		public ISqlFormatter Formatter
		{
			get { return _sqlFormatter; }
		}
		#endregion
	}
}