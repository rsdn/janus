namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class ForumViewerCommandProvider : ResourceCommandProvider
	{
		public ForumViewerCommandProvider()
			: base(
				"Rsdn.Janus.Features.ForumViewer.Commands.ForumViewer.ForumViewerCommands.xml",
				"Rsdn.Janus.Features.ForumViewer.Commands.ForumViewer.ForumViewerCommandResources") { }
	}
}