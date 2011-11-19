using System;
using System.Data.Common;
using System.Data.SQLite;

using BLToolkit.Data.DataProvider;

namespace Rsdn.Janus.Sqlite
{
	[JanusDBDriver(
		DriverName,
		"Rsdn.Janus.Sqlite.Resources",
		"DriverDisplayName",
		"DriverDescription",
		LockRequired = true)]
	internal class SqliteDriver : IDBDriver
	{
		public const string DriverName = "SQLite";
		private readonly IServiceProvider _provider;
		private readonly SqliteSqlFormatter _sqlFormatter = new SqliteSqlFormatter();

		public SqliteDriver(IServiceProvider provider)
		{
			_provider = provider;
		}

		#region IDBDriver Members
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

		/// <summary>
		/// Получить драйвер схемы.
		/// </summary>
		/// <returns></returns>
		public IDBSchemaDriver CreateSchemaDriver()
		{
			return new SqliteSchemaDriver(_provider);
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
		/// Создать провайдер для BLToolkit.
		/// </summary>
		public DataProviderBase CreateDataProvider()
		{
			return new SQLiteDataProvider();
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