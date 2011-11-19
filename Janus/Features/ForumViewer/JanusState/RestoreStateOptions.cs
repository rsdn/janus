using System;

namespace Rsdn.Janus
{
	[Flags]
	public enum RestoreStateOptions
	{
		None = 0x0,
		Markers = 0x1,
		ClearMarkers = 0x2,
		ReadedMessages = 0x4,
		StoreUnreaded = 0x8,
		Favorites = 0x10,
		ClearFavorites = 0x20
	}
}