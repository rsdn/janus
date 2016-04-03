using System;

using CodeJam.Collections;
using CodeJam.Extensibility;
using CodeJam.Extensibility.Instancing;
using CodeJam.Extensibility.Registration;

namespace Rsdn.Janus
{
	using DriverSvc = IRegKeyedElementsService<string, JanusDBDriverInfo>;

	[Service(typeof (IDBDriverManager))]
	internal class DBDriverManager : IDBDriverManager
	{
		private readonly IServiceProvider _provider;
		private readonly ILazyDictionary<string, IDBDriver> _cache;

		public DBDriverManager(IServiceProvider provider)
		{
			_provider = provider;
			_cache = LazyDictionary.Create<string, IDBDriver>(
				name =>
				{
					var svc = _provider.GetService<DriverSvc>();
					if (svc == null || !svc.ContainsElement(name))
						throw new ArgumentException($"Unknown driver '{name}'");
					return (IDBDriver)svc.GetElement(name).Type.CreateInstance(_provider);
				},
				true);
		}

		#region IDBDriverManager Members
		/// <summary>
		/// Возвращает экземпляр драйвера по его имени.
		/// </summary>
		public IDBDriver GetDriver(string driverName)
		{
			return _cache[driverName];
		}

		/// <summary>
		/// Возвращает список зарегистрированных драйверов.
		/// </summary>
		public JanusDBDriverInfo[] GetRegisteredDriverInfos()
		{
			var svc = _provider.GetService<DriverSvc>();
			return
				svc == null
					? Array<JanusDBDriverInfo>.Empty
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
