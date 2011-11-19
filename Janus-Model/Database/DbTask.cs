using System;

namespace Rsdn.Janus
{
	[Flags]
	public enum DBTask
	{
		None = 0x0000,
		Compact = 0x0001,
	}
}