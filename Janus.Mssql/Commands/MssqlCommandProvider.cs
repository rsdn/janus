namespace Rsdn.Janus.Mssql
{
	[CommandProvider]
	internal sealed class FirebirdCommandProvider : ResourceCommandProvider
	{
		public FirebirdCommandProvider()
			: base("Rsdn.Janus.Mssql.Commands.MssqlCommands.xml", "Rsdn.Janus.Mssql.Resources") { }
	}
}