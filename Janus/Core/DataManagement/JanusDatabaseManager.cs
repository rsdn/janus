using System;
using System.Data;

using LinqToDB.Data;
using LinqToDB.DataProvider;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(IJanusDatabaseManager))]
	internal class JanusDatabaseManager : ServiceConsumer, IJanusDatabaseManager
	{
#pragma warning disable 0649
		[ExpectService]
		private IDBDriverManager _drvManager;
#pragma warning restore 0649

		private readonly string _currentDriverName;
		private readonly IDBDriver _currentDriver;
		private readonly IDataProvider _bltDataProvider;
		private readonly string _connectionString;
		private readonly IJanusRWLock _rwLock;

		public JanusDatabaseManager(IServiceProvider provider)
			: base(provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			_currentDriverName = Config.Instance.DbDriver;
			var info = _drvManager.GetDriverInfo(_currentDriverName);
			_currentDriver = _drvManager.GetDriver(_currentDriverName);
			_bltDataProvider = _currentDriver.CreateDataProvider();
			_connectionString = Config.Instance.ConnectionString;
			_rwLock = info.LockRequired ? (IJanusRWLock)new StandardJanusRWLock() : EmptyJanusRWLock.LockInstance;
		}

		#region IJanusDatabaseManager Members
		public string GetCurrentDriverName()
		{
			return _currentDriverName;
		}

		public IDBDriver GetCurrentDriver()
		{
			return _currentDriver;
		}

		public string GetCurrentConnectionString()
		{
			return _connectionString;
		}

		/// <summary>
		/// Возвращает лок для работы с БД. Необходим для движков, которые
		/// не поддерживают нормальный параллельный доступ (Jet, Sqlite).
		/// </summary>
		public IJanusRWLock GetLock()
		{
			return _rwLock;
		}

		public IJanusDataContext CreateDBContext()
		{
			return new JanusDataContext(_bltDataProvider, _connectionString);
		}
		#endregion

		#region JanusDataContext class
		private class JanusDataContext : DataConnection, IJanusDataContext
		{
			public JanusDataContext(IDataProvider dataProvider, string connectionString) : base(dataProvider, connectionString)
			{
				CommandTimeout = 0;
			}

			IDbTransaction IJanusDataContext.BeginTransaction()
			{
				BeginTransaction();
				return Transaction;
			}
		}
		#endregion
	}
}
