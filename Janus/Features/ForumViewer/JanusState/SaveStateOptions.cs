using System;

namespace Rsdn.Janus
{
	[Flags]
	public enum SaveStateOptions
	{
		None = 0x0,
		Markers = 0x1,
		ReadedMessages = 0x2,
		Favorites = 0x4
	}
}