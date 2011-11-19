using Rsdn.Janus;
using Rsdn.SmartApp;

[assembly:Module(typeof(FbSupportModule), "DBSupport.Firebird", "Firebird Support Module",
		"Rsdn.Janus.Core.DataSchemaManagement.Drivers.Firebird.FbModuleIcon.png")]

namespace Rsdn.Janus
{
	/// <summary>
	/// Модуль поддержки MS SQL Server.
	/// </summary>
	[DBDriver(FbSupportModule.DriverName, "Firebird")]
	internal class FbSupportModule: IModule
	{
		public const string DriverName = "Fb";

		#region IModule Members

		public void Initialize(IServiceProvider serviceProvider)
		{
			serviceProvider.GetService<IDBDriverManager>().InstallDriver(
				DriverName, new FBDriver());
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{ }

		#endregion
	}
}
