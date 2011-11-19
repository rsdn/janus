using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class FavoritesToolbarProvider : ResourceMenuProvider
	{
		public FavoritesToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.Favorites.Menu.FavoritesToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}