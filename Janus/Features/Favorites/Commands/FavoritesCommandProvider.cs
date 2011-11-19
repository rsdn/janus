namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class FavoritesCommandProvider : ResourceCommandProvider
	{
		public FavoritesCommandProvider()
			: base(
				"Rsdn.Janus.Features.Favorites.Commands.FavoritesCommands.xml",
				"Rsdn.Janus.SR") { }
	}
}