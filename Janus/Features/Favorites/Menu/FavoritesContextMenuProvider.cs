using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class FavoritesContextMenuProvider : ResourceMenuProvider
	{
		public FavoritesContextMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.Favorites.Menu.FavoritesContextMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}