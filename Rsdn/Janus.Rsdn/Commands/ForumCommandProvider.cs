namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class ForumCommandProvider : ResourceCommandProvider
	{
		public ForumCommandProvider()
			: base(
				"Rsdn.Janus.Commands.ForumCommands.xml",
				"Rsdn.Janus.Commands.ForumCommandResources") { }
	}
}