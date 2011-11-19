namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class DatabaseCommandProvider : ResourceCommandProvider
	{
		public DatabaseCommandProvider()
			: base("Rsdn.Janus.Core.DataManagement.Commands.DatabaseCommands.xml", "Rsdn.Janus.SR") { }
	}
}