using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class TagLineContextMenuProvider : ResourceMenuProvider
	{
		public TagLineContextMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.TagLine.Menu.TagLineContextMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}