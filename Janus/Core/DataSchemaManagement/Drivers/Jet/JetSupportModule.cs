using Rsdn.Janus;
using Rsdn.SmartApp;

[assembly:Module(typeof(JetSupportModule), "DBSupport.Jet", "Jet Support Module",
		"Rsdn.Janus.Core.DataSchemaManagement.Drivers.Jet.JetModuleIcon.png")]

namespace Rsdn.Janus
{
	/// <summary>
	/// Модуль поддержки БД Jet.
	/// </summary>
	[DBDriver(JetSupportModule.DriverName, "Access")]
	internal class JetSupportModule : IModule
	{
		public const string DriverName = "Jet";

		#region IModule Members

		// Регистрация предоставляемых возможностей.
		public void Initialize(IServiceProvider serviceProvider)
		{
			serviceProvider.GetService<IDBDriverManager>().InstallDriver(
				DriverName, new JetDriver());
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{ }

		#endregion
	}
}
