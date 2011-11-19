namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class ForumCommandProvider : ResourceCommandProvider
	{
		public ForumCommandProvider()
			: base(
				"Rsdn.Janus.Features.ForumViewer.Commands.Forum.ForumCommands.xml",
				"Rsdn.Janus.Features.ForumViewer.Commands.Forum.ForumCommandResources") { }
	}
}