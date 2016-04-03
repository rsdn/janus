using System;
using System.Reactive;
using System.Reactive.Disposables;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд синхронизации.
	/// </summary>
	[CommandTarget]
	internal sealed class SynchronizationCommandTarget : CommandTarget
	{
		public SynchronizationCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		[CommandStatusSubscriber("Janus.Synchronization.Synchronize")]
		public IDisposable SubscribeStatusChangedCore(
			IServiceProvider serviceProvider, Action handler)
		{
			var syncSvc = serviceProvider.GetService<ISynchronizer>();
			if (syncSvc != null)
			{
				var startSyncSubscription = syncSvc.StartSync.Subscribe(
					Observer.Create<EventArgs>(arg => handler()));
				var endSyncSubscription = syncSvc.EndSync.Subscribe(
					Observer.Create<EndSyncEventArgs>(arg => handler()));
				return
					Disposable.Create(
						() =>
						{
							startSyncSubscription.Dispose();
							endSyncSubscription.Dispose();
						});
			}
			return Disposable.Empty;
		}

		[CommandExecutor("Janus.Synchronization.Synchronize")]
		public void ExecuteSynchronize(ICommandContext context)
		{
			context.PeriodicSync(Config.Instance.SyncThreadPriority, true);
			//ApplicationManager.Instance.MainForm.ResetAutoSyncTimer();
		}

		[CommandStatusGetter("Janus.Synchronization.Synchronize")]
		public CommandStatus QuerySynchronizeStatus(ICommandContext context)
		{
			var syncSvc = context.GetService<ISynchronizer>();

			if (syncSvc == null)
				return CommandStatus.Unavailable;

			return syncSvc.IsActive() ? CommandStatus.Disabled : CommandStatus.Normal;
		}
	}
}