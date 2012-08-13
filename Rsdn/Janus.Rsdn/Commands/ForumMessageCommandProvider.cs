namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class ForumMessageCommandProvider : ResourceCommandProvider
	{
		public ForumMessageCommandProvider()
			: base(
				"Rsdn.Janus.Commands.ForumMessageCommands.xml",
				"Rsdn.Janus.Properties.Resources") { }
	}
}