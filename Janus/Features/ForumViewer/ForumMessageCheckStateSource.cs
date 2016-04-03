using System;
using System.Linq;
using System.Reactive;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Extensibility;
using CodeJam.Extensibility.EventBroker;

using Disposable = CodeJam.Disposable;

namespace Rsdn.Janus
{
	[CheckStateSource]
	internal sealed class ForumMessageCheckStateSource : CheckStateSource
	{
		[CheckStateGetter("Janus.Forum.MessagesRepliesAutoReadMark")]
		public CheckState? GetMessagesRepliesAutoReadMarkCheckState(IServiceProvider provider)
		{
			var activeMessageSvc = provider.GetService<IActiveMessagesService>();
			if (activeMessageSvc != null && activeMessageSvc.ActiveMessages.Any())
			{
				bool? state = null;
				foreach (var message in activeMessageSvc.ActiveMessages)
				{
					if (state == null)
						state = message.ReadReplies;
					else if (state != message.ReadReplies)
						return CheckState.Indeterminate;
				}
				if (state != null)
					return state.Value ? CheckState.Checked : CheckState.Unchecked;
			}
			return null;
		}

		[CheckStateSubscriber("Janus.Forum.MessagesRepliesAutoReadMark")]
		public IDisposable ActiveMessagesChangedSubscriber(IServiceProvider provider, Action handler)
		{
			var eventBroker = provider.GetRequiredService<IEventBroker>();
			var afterForumEntryChangedSubscription = eventBroker
				.Subscribe(
					ForumEventNames.AfterForumEntryChanged,
					Observer.Create<ForumEntryChangedEventArgs>(
					args =>
					{
						if ((args.ChangeType & ForumEntryChangeType.AutoReadMark) != 0)
							handler();
					}));

			IDisposable activeMessagesChangedSubscription;
			var activeMessageService = provider.GetService<IActiveMessagesService>();
			if (activeMessageService != null)
			{
				EventHandler statusUpdater = (sender, e) => handler();
				activeMessageService.ActiveMessagesChanged += statusUpdater;
				activeMessagesChangedSubscription = Disposable.Create(
					() => activeMessageService.ActiveMessagesChanged -= statusUpdater);
			}
			else
				activeMessagesChangedSubscription = Disposable.Empty;

			return
				new[]
				{
					afterForumEntryChangedSubscription,
					activeMessagesChangedSubscription
				}
				.ToArray()
				.Merge();
		}
	}
}