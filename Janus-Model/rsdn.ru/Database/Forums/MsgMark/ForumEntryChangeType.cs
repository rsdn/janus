using System;

namespace Rsdn.Janus
{
	[Flags]
	public enum ForumEntryChangeType
	{
		ReadMark = 0x01,
		AutoReadMark = 0x02,
		ForumSubscription = 0x04,
		Synchronized = ReadMark
	}
}