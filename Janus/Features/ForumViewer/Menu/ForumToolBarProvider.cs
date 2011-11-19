using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class ForumToolbarProvider : ResourceMenuProvider
	{
		public ForumToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.Menu.ForumToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}