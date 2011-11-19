namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class ForumsCommandProvider : ResourceCommandProvider
	{
		public ForumsCommandProvider()
			: base(
				"Rsdn.Janus.Features.ForumViewer.Commands.Forums.ForumsCommands.xml",
				"Rsdn.Janus.Features.ForumViewer.Commands.Forums.ForumsCommandResources") { }
	}
}