using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	using DriverSvc = IRegKeyedElementsService<string, JanusDBDriverInfo>;

	[Service(typeof (IDBDriverManager))]
	internal class DBDriverManager : IDBDriverManager
	{
		private readonly IServiceProvider _provider;
		private readonly ElementsCache<string, IDBDriver> _cache;

		public DBDriverManager(IServiceProvider provider)
		{
			_provider = provider;
			_cache = new ElementsCache<string, IDBDriver>(
				name =>
				{
					var svc = _provider.GetService<DriverSvc>();
					if (svc == null || !svc.ContainsElement(name))
						throw new ArgumentException("Unknown driver '{0}'".FormatStr(name));
					return (IDBDriver)svc.GetElement(name).Type.CreateInstance(_provider);
				});
		}

		#region IDBDriverManager Members
		/// <summary>
		/// Возвращает экземпляр драйвера по его имени.
		/// </summary>
		public IDBDriver GetDriver(string driverName)
		{
			return _cache.Get(driverName);
		}

		/// <summary>
		/// Возвращает список зарегистрированных драйверов.
		/// </summary>
		public JanusDBDriverInfo[] GetRegisteredDriverInfos()
		{
			var svc = _provider.GetService<DriverSvc>();
			return
				svc == null
					? EmptyArray<JanusDBDriverInfo>.Value
					: svc.GetRegisteredElements();
		}

		public JanusDBDriverInfo GetDriverInfo(string driverName)
		{
			var svc = _provider.GetRequiredService<DriverSvc>();
			return svc.GetElement(driverName);
		}
		#endregion
	}
}
