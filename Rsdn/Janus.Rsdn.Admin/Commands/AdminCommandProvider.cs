namespace Rsdn.Janus.Admin
{
	[CommandProvider]
	internal class AdminCommandProvider : ResourceCommandProvider
	{
		public AdminCommandProvider()
			: base(
				"Rsdn.Janus.Admin.Commands.AdminCommands.xml",
				"Rsdn.Janus.Admin.Commands.AdminCommandResources")
		{}
	}
}
