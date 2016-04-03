using System;

using CodeJam.Extensibility;

using JetBrains.Annotations;

using Rsdn.Janus.Core.Synchronization.UI.Notification;
using Rsdn.Janus.Framework;

namespace Rsdn.Janus
{
	[ActivePart]
	public class SyncNotificationActivePart : ActivePartBase
	{
		private readonly ISynchronizer _synchronizer;
		private IDisposable _startSyncSubscription;
		private IDisposable _endSyncSubscription;

		public SyncNotificationActivePart([NotNull] IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_synchronizer = ServiceProvider.GetService<ISynchronizer>();
		}

		#region Overrides of ActivePartBase

		protected override void ActivateCore()
		{
			if (_synchronizer == null)
				return;
			if (ServiceProvider.GetService<INotificationService>() == null)
				return;

			_startSyncSubscription = _synchronizer.StartSync.Subscribe(StartSync);
			_endSyncSubscription = _synchronizer.EndSync.Subscribe(EndSync);
		}

		protected override void PassivateCore()
		{
			if (_startSyncSubscription != null)
			{
				_startSyncSubscription.Dispose();
				_startSyncSubscription = null;
			}
			if (_endSyncSubscription != null)
			{
				_endSyncSubscription.Dispose();
				_endSyncSubscription = null;
			}
		}

		#endregion

		#region Private Members

		private void StartSync(EventArgs args)
		{
			//ServiceProvider.ShowNotification(
			//    SyncNotificationResources.SyncAuto,
			//    SyncNotificationResources.SyncStart,
			//    NotificationType.Info);
			ServiceProvider.ShowNotification(
				SyncNotificationResources.SyncManual,
				SyncNotificationResources.SyncStart,
				NotificationType.Info);
		}

		private void EndSync(EndSyncEventArgs args)
		{
			switch (args.Result)
			{
				case SyncResult.Finished:
					if (args.StatisticsContainer.IsEmpty())
						return;

					ServiceProvider.ShowNotification(
						SyncNotificationResources.SyncFinish,
						args.StatisticsContainer.GetFormattedValues(ServiceProvider),
						NotificationType.Info);

					if (Config.Instance.SoundConfig.MakeSound)
						Beeper.DoBeep(Config.Instance.SoundConfig.SoundFile);
					break;

				case SyncResult.Failed:
					ServiceProvider.ShowNotification(
						SyncNotificationResources.SyncError,
						args.Exception.Message,
						NotificationType.Error);
					break;
			}
		}

		#endregion
	}
}