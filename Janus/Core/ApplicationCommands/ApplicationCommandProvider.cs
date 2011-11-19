namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class ApplicationCommandProvider : ResourceCommandProvider
	{
		public ApplicationCommandProvider()
			: base(
				"Rsdn.Janus.Core.ApplicationCommands.ApplicationCommands.xml",
				"Rsdn.Janus.Core.ApplicationCommands.ApplicationCommandResources") { }
	}
}