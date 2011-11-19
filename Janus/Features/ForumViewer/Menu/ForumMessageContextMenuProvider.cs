using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class ForumMessageContextMenuProvider : ResourceMenuProvider
	{
		public ForumMessageContextMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.Menu.ForumMessageContextMenu.xml", 
				"Rsdn.Janus.SR") { }
	}
}