using Rsdn.Janus;
using Rsdn.SmartApp;

[assembly:Module(typeof(SqliteSupportModule), "DBSupport.Sqlite", "SQLite Support Module",
		"Rsdn.Janus.Core.DataSchemaManagement.Drivers.Sqlite.SqliteModuleIcon.png")]

namespace Rsdn.Janus
{
	/// <summary>
	/// Модуль поддержки SQLite.
	/// </summary>
	[DBDriver(SqliteSupportModule.DriverName, "SQLite")]
	internal class SqliteSupportModule : IModule
	{
		public const string DriverName = "SQLite";

		#region IModule Members

		public void Initialize(IServiceProvider serviceProvider)
		{
			serviceProvider.GetService<IDBDriverManager>().InstallDriver(
				DriverName, new SqliteDriver());
		}

		#endregion

		#region IDisposable Members
		
		public void Dispose()
		{ }

		#endregion
	}
}
