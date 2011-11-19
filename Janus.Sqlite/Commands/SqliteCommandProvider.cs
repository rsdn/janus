namespace Rsdn.Janus.Sqlite
{
	[CommandProvider]
	internal sealed class SqliteCommandProvider : ResourceCommandProvider
	{
		public SqliteCommandProvider()
			: base("Rsdn.Janus.Sqlite.Commands.SqliteCommands.xml", "Rsdn.Janus.Sqlite.Resources") { }
	}
}