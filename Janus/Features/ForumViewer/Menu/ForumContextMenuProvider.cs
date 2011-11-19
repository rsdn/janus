using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class ForumContextMenuProvider : ResourceMenuProvider
	{
		public ForumContextMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.Menu.ForumContextMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}