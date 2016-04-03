using System;
using System.Reactive.Subjects;

using CodeJam.Extensibility;
using CodeJam.Extensibility.EventBroker;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[Service(typeof(IForumsAggregatesService))]
	internal sealed class ForumsAggregatesService : IForumsAggregatesService, IDisposable
	{
		private readonly Subject<EventArgs> _aggregatesChanged = new Subject<EventArgs>();
		private readonly IDisposable _eventsSubscription;

		public ForumsAggregatesService([NotNull] IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));

			UpdateAggregates();

			_eventsSubscription = EventBrokerHelper.SubscribeEventHandlers(this, provider);
		}

		#region Implementation of IForumsAggregatesService

		public int MessagesCount { get; private set; }

		public int UnreadMessagesCount { get; private set; }

		public int UnreadRepliesToMeCount { get; private set; }

		public IObservable<EventArgs> AggregatesChanged => _aggregatesChanged;
		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			_eventsSubscription.Dispose();
		}

		#endregion

		#region Private Members

		private void UpdateAggregates()
		{
			MessagesCount = 0;
			UnreadRepliesToMeCount = 0;
			UnreadMessagesCount = 0;

			foreach (var forum in Forums.Instance.ForumList)
			{
				MessagesCount += forum.MessagesCount;
				UnreadRepliesToMeCount += forum.RepliesToMeUnread;
				UnreadMessagesCount += forum.Unread;
			}
		}

		[EventHandler(ForumEventNames.AfterForumEntryChanged)]
		private void AfterForumEntryChanged(ForumEntryChangedEventArgs arg)
		{
			if ((arg.ChangeType & ForumEntryChangeType.ReadMark) == 0)
				return;

			UpdateAggregates();

			_aggregatesChanged.OnNext(EventArgs.Empty);
		} 
		
		#endregion
	}
}