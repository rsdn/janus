namespace Rsdn.Janus.Sqlite
{
	[ExtensionInfoProvider]
	internal class SqliteExtensionInfoProvider : ExtensionInfoProviderBase
	{
		public SqliteExtensionInfoProvider()
			: base("Rsdn.Janus.Sqlite.SqliteExtensionIcon.png")
		{}

		protected override string GetDisplayName()
		{
			return Resources.ExtensionDisplayName;
		}
	}
}