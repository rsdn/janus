using System;
using System.Data;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

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
		private readonly DataProviderBase _bltDataProvider;
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
			_rwLock = info.LockRequired ? (IJanusRWLock)new StandardJanusRWLock() : EmptyJanusRWLock.Instance;
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
			var db = new JanusDataContext(_bltDataProvider, _connectionString);
			db.InitCommand += (sender, ea) => ea.Command.CommandTimeout = 0;
			return db;
		}
		#endregion

		#region JanusDataContext class
		private class JanusDataContext : DbManager, IJanusDataContext
		{
			public JanusDataContext(DataProviderBase dataProvider, string connectionString) 
				: base(dataProvider, connectionString)
			{}

			IDbTransaction IJanusDataContext.BeginTransaction()
			{
				return BeginTransaction().Transaction;
			}
		}
		#endregion
	}
}
