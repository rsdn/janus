using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class SearchToolbarProvider : ResourceMenuProvider
	{
		public SearchToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.Search.Menu.SearchToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}