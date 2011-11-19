using Rsdn.Janus;
using Rsdn.SmartApp;

[assembly:Module(typeof(MssqlSupportModule),
	"DBSupport.Mssql", "MS SQL Server Support Module",
	"Rsdn.Janus.Core.DataSchemaManagement.Drivers.Mssql.MssqlModuleIcon.png")]

namespace Rsdn.Janus
{
	/// <summary>
	/// Модуль поддержки MS SQL Server.
	/// </summary>
	[DBDriver(MssqlSupportModule.DriverName, "MS SQL Server")]
	internal class MssqlSupportModule : IModule
	{
		public const string DriverName = "MSSql";

		#region IModule Members
		public void Initialize(IServiceProvider serviceProvider)
		{
			serviceProvider.GetService<IDBDriverManager>().InstallDriver(
				DriverName, new MssqlDriver());
		}
		#endregion

		#region IDisposable Members

		public void Dispose()
		{ }

		#endregion
	}
}
