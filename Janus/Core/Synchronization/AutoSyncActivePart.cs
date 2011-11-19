using System;
using System.ComponentModel;
using System.Windows.Forms;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[ActivePart]
	internal sealed class AutoSyncActivePart : ActivePartBase
	{
		private readonly ISynchronizer _synchronizer;
		private readonly AsyncOperation _uiAsyncOperation;
		private bool _isEnabled;
		private Timer _timer;
		private IDisposable _startSyncSubscription;
		private IDisposable _endSyncSubscription;

		public AutoSyncActivePart([NotNull] IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_synchronizer = ServiceProvider.GetService<ISynchronizer>();
			_uiAsyncOperation = ServiceProvider.GetRequiredService<IUIShell>().CreateUIAsyncOperation();
		}

		#region Overrides of ActivePartBase

		protected override void ActivateCore()
		{
			if (_synchronizer == null)
				return;

			if(Config.Instance.AutoSync)
				EnableAutoSync();

			Config.Instance.ConfigChanged += ConfigChanged;
		}

		protected override void PassivateCore()
		{
			if (_synchronizer == null)
				return;

			Config.Instance.ConfigChanged -= ConfigChanged;
			
			if (_isEnabled)
				DisableAutoSync();
		}

		#endregion

		#region Private Members

		private void EnableAutoSync()
		{
			_uiAsyncOperation.Send(
				() =>
				{
					_timer = new Timer { Interval = Config.Instance.AutoSyncInterval * 1000 };
					_timer.Tick += TimerTick;
					_timer.Start();
				});

			_startSyncSubscription = _synchronizer.StartSync.Subscribe(args => _timer.Stop());
			_endSyncSubscription = _synchronizer.EndSync.Subscribe(args => _timer.Start());

			_isEnabled = true;
		}

		private void DisableAutoSync()
		{
			_startSyncSubscription.Dispose();
			_endSyncSubscription.Dispose();

			_uiAsyncOperation.Send(
				() =>
				{
					_timer.Stop();
					_timer.Tick -= TimerTick;
					_timer.Dispose();
				});

			_isEnabled = false;
		}

		private void TimerTick(object sender, EventArgs e)
		{
			ServiceProvider.PeriodicSyncIfAvailable(Config.Instance.SyncThreadPriority, false);
		}

		private void ConfigChanged(object sender, EventArgs e)
		{
			if (!_isEnabled && Config.Instance.AutoSync)
				EnableAutoSync();
			else if (_isEnabled && !Config.Instance.AutoSync)
				DisableAutoSync();
			else if (_isEnabled && _timer.Interval / 1000 != Config.Instance.AutoSyncInterval)
				_timer.Interval = Config.Instance.AutoSyncInterval * 1000;
		}

		#endregion
	}
}