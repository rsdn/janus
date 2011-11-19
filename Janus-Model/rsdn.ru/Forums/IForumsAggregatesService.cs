using System;

namespace Rsdn.Janus
{
	public interface IForumsAggregatesService
	{
		int MessagesCount { get; }
		int UnreadMessagesCount { get; }
		int UnreadRepliesToMeCount { get; }
		IObservable<EventArgs> AggregatesChanged { get; }
	}
}