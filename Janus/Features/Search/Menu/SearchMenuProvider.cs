using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class SearchMenuProvider : ResourceMenuProvider
	{
		public SearchMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.Search.Menu.SearchMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}