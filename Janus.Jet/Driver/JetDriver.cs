using System;
using System.Data.Common;
using System.Data.OleDb;

using LinqToDB.DataProvider;
using LinqToDB.DataProvider.Access;

namespace Rsdn.Janus.Jet
{
	/// <summary>
	/// Драйвер БД JET.
	/// </summary>
	[JanusDBDriver(
		DriverName,
		"Rsdn.Janus.Jet.Resources",
		"DriverDisplayName",
		"DriverDescription",
		LockRequired = true)]
	internal class JetDriver : IDBDriver
	{
		public const string DriverName = "Jet";
		private readonly IServiceProvider _provider;
		private readonly JetSqlFormatter _sqlFormatter = new JetSqlFormatter();

		public JetDriver(IServiceProvider provider)
		{
			_provider = provider;
		}

		#region IDBDriver Members
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

		/// <summary>
		/// Получить драйвер схемы.
		/// </summary>
		/// <returns></returns>
		public IDBSchemaDriver CreateSchemaDriver()
		{
			return new JetSchemaDriver();
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
		/// Создать провайдер для BLToolkit.
		/// </summary>
		public IDataProvider CreateDataProvider()
		{
			return new AccessDataProvider();
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