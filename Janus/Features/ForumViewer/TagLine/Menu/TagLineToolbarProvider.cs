using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class TagLineToolbarProvider : ResourceMenuProvider
	{
		public TagLineToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.TagLine.Menu.TagLineToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}