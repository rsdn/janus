using System;
using System.Reactive;

using CodeJam.Extensibility;
using CodeJam.Extensibility.EventBroker;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[ActivePart]
	public class SyncRefreshActivePart : ActivePartBase
	{
		private readonly ISynchronizer _synchronizer;
		private readonly IEventBroker _eventBroker;
		private IDisposable _endSyncSubscription;

		public SyncRefreshActivePart([NotNull] IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_synchronizer = ServiceProvider.GetRequiredService<ISynchronizer>();
			_eventBroker = ServiceProvider.GetRequiredService<IEventBroker>();
		}

		protected override void ActivateCore()
		{
			_endSyncSubscription = _synchronizer.EndSync.Subscribe(
				Observer.Create<EndSyncEventArgs>(
					arg =>
					{
						if (arg.Result != SyncResult.Finished || arg.StatisticsContainer.IsEmpty())
							return;

						_eventBroker.Fire(
							ForumEventNames.BeforeForumEntryChanged,
							new ForumEntryChangedEventArgs(
								new[] { ForumEntryIds.AllForums },
								ForumEntryChangeType.Synchronized));

						ServiceProvider.GetRequiredService<IUIShell>().RefreshData();

						_eventBroker.Fire(
							ForumEventNames.AfterForumEntryChanged,
							new ForumEntryChangedEventArgs(
								new[] { ForumEntryIds.AllForums },
								ForumEntryChangeType.Synchronized));
					}));
		}

		protected override void PassivateCore()
		{
			_endSyncSubscription.Dispose();
		}
	}
}