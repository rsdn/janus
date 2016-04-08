using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Collections;
using CodeJam.Extensibility;
using CodeJam.Services;

using JetBrains.Annotations;

using Rsdn.Janus.Core.Synchronization;
using Rsdn.Janus.Log;

using Disposable = System.Reactive.Disposables.Disposable;

namespace Rsdn.Janus
{
	/// <summary>
	/// Реализует синхронизацию с веб-сервисом.
	/// </summary>
	[Service(typeof(ISynchronizer))]
	internal class Synchronizer : ISynchronizer, IDisposable
	{
		private readonly IServiceProvider _provider;
		private readonly AsyncOperation _uiAsyncOp;
		private readonly Subject<EventArgs> _startSync = new Subject<EventArgs>();
		private readonly Subject<EndSyncEventArgs> _endSync = new Subject<EndSyncEventArgs>();
		private SyncForm _lastSyncFormInstance;
		private bool _isActive;
		private readonly ExtensionsCache<SyncProviderInfo, string, ISyncProvider> _syncProvidersCache;
		private readonly CompositeDisposable _disposables;

		public Synchronizer([NotNull] IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));

			_provider = provider;

			_syncProvidersCache = new ExtensionsCache<SyncProviderInfo, string, ISyncProvider>(provider);

			var syncContext = _provider.GetRequiredService<IUIShell>().UISyncContext;
			StartSync = _startSync.ObserveOn(syncContext);
			EndSync = _endSync.ObserveOn(syncContext);
			_uiAsyncOp = _provider.GetRequiredService<IUIShell>().CreateUIAsyncOperation();

			_disposables =
				new CompositeDisposable(
					_startSync.CompleteHelper(),
					_endSync.CompleteHelper(),
					Disposable.Create(_uiAsyncOp.OperationCompleted));
		}

		#region ISynchronizer Members

		public bool IsActive()
		{
			return _isActive;
		}

		public bool IsAvailable()
		{
			return
				_syncProvidersCache
					.GetAllExtensions()
					.All(syncProv => syncProv.IsAvailable());
		}

		/// <summary>
		/// Start synchronization
		/// All subscribers should be already setup for events
		/// It uses Logger to notify about progress
		/// </summary>
		public IStatisticsContainer SyncPeriodic(bool activateUI)
		{
			return PerformSyncSession(
				context =>
					_syncProvidersCache
						.GetExtensionInfos()
						.ForEach(info => PerformSyncProvider(context, info.Key, null)),
				activateUI);
		}

		public IStatisticsContainer SyncSpecific(string providerName, string taskName, bool activateUI)
		{
			if (providerName == null)
				throw new ArgumentNullException(nameof(providerName));
			if (taskName == null)
				throw new ArgumentNullException(nameof(taskName));
			var prov = _syncProvidersCache.GetExtension(providerName);
			if (prov == null)
				throw new ArgumentException("Unknown sync provider");

			return
				PerformSyncSession(
					context =>
						PerformSyncProvider(
							context,
							providerName,
							taskName),
					activateUI);
		}

		public IObservable<EventArgs> StartSync { get; }

		public IObservable<EndSyncEventArgs> EndSync { get; }
		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			_disposables.Dispose();
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Инициализация переменных и событий для синхронизации. 
		/// </summary>
		private bool InitStartSync()
		{
			if (IsActive())
			{
				_provider.LogWarning(SyncResources.SyncAlreadyActive);
				return false;
			}

			_isActive = true;
			OnStartSync();

			return true;
		}

		private void PerformSyncProvider(
			ISyncContext context,
			string provName,
			string taskName)
		{
			var syncProv = _syncProvidersCache.GetExtension(provName);
			if (syncProv == null)
				throw new ArgumentException("Unknown provider.");

			if (taskName == null)
				syncProv.SyncPeriodicTasks(context);
			else
				syncProv.SyncTask(context, taskName);
		}

		private IStatisticsContainer PerformSyncSession(
			Action<ISyncContext> syncProc,
			bool activateUI)
		{
			var stats = new StatisticsContainer();

			if (!InitStartSync())
				return stats;

			var svcManager = new ServiceContainer(_provider);
			SyncForm syncForm = null;
			var result = SyncResult.Failed;
			Exception failException = null;
			try
			{
				var context = new SyncContext(
					svcManager,
					stats,
					// ReSharper disable AccessToModifiedClosure
					() => syncForm != null && syncForm.IsCancelled);
					// ReSharper restore AccessToModifiedClosure	

				if (_lastSyncFormInstance != null)
					_uiAsyncOp.Send(_lastSyncFormInstance.Dispose);
				if (Config.Instance.ShowSyncWindow)
				{
					_uiAsyncOp.Send(
						() =>
						{
							syncForm =
								new SyncForm(context)
								{
									WindowState = activateUI 
										? FormWindowState.Normal 
										: FormWindowState.Minimized
								};
							syncForm.Show();
						});
					syncForm.Closed += (sender, args) => _lastSyncFormInstance = null;
					_lastSyncFormInstance = syncForm;
					svcManager.Publish<ISyncProgressVisualizer>(syncForm);
					svcManager.Publish<ITaskIndicatorProvider>(syncForm);
					svcManager.Publish<ISyncErrorInformer>(syncForm);
				}

				syncProc(context);

				if (!stats.IsEmpty())
					_provider.LogInfo(stats.GetFormattedValues(_provider));

				result = SyncResult.Finished;
			}
			catch (UserCancelledException ex)
			{
				_provider.LogWarning(SyncResources.SyncWarning.FormatWith(ex.Message));
				result = SyncResult.Aborted;
			}
			catch (Exception ex)
			{
				_provider.LogError(SyncResources.SyncError.FormatWith());
				result = SyncResult.Failed;
				failException = ex;
			}
			finally
			{
				_isActive = false;
				OnEndSync(new EndSyncEventArgs(stats, result, failException));
				if (syncForm != null)
					_uiAsyncOp.Send(syncForm.TryClose);
			}
			return stats;
		}

		private void OnStartSync()
		{
			_startSync.OnNext(EventArgs.Empty);
		}

		private void OnEndSync(EndSyncEventArgs arg)
		{
			_endSync.OnNext(arg);
		}
		#endregion
	}
}