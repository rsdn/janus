namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class TagLineCommandProvider : ResourceCommandProvider
	{
		public TagLineCommandProvider()
			: base(
				"Rsdn.Janus.Features.ForumViewer.TagLine.Commands.TagLineCommands.xml",
				"Rsdn.Janus.SR") { }
	}
}