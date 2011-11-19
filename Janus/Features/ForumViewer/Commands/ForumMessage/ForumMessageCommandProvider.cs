namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class ForumMessageCommandProvider : ResourceCommandProvider
	{
		public ForumMessageCommandProvider()
			: base(
				"Rsdn.Janus.Features.ForumViewer.Commands.ForumMessage.ForumMessageCommands.xml",
				"Rsdn.Janus.Features.ForumViewer.Commands.ForumMessage.ForumMessageCommandResources") { }
	}
}