using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using CodeJam.Extensibility;
using CodeJam.Services;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[Service(typeof(INotifyIconService))]
	internal class NotifyIconService : INotifyIconService, IDisposable
	{
		private readonly ServiceContainer _serviceManager;
		private readonly AsyncOperation _uiAsyncOperation;
		private readonly IDefaultCommandService _defaultCommandService;
		private NotifyIcon _notifyIcon;
		private ContextMenuStrip _trayMenuStrip;
		private StripMenuGenerator _trayMenuGenerator;
		private bool _enabled;
		private string _contextMenuName;

		public NotifyIconService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			_serviceManager = new ServiceContainer(serviceProvider);
			_defaultCommandService = new DefaultCommandService("Janus.Application.ShowMainForm");
			_serviceManager.Publish(_defaultCommandService);

			_uiAsyncOperation = _serviceManager
				.GetRequiredService<IUIShell>()
				.CreateUIAsyncOperation();

			_uiAsyncOperation.Send(
				() =>
				{
					_notifyIcon = new NotifyIcon();
					_notifyIcon.DoubleClick += NotifyIconDefaultAction;
					_notifyIcon.BalloonTipClicked += NotifyIconDefaultAction;
					Ticker.Instance.DoubleClick += NotifyIconDefaultAction;
				});
		}

		#region INotifyIconService Members

		public void ShowBalloonTip(
			string tipTitle,
			[NotNull] string tipText,
			NotificationType notificationType,
			int timeout)
		{
			if (tipText == null)
				throw new ArgumentNullException(nameof(tipText));

			_uiAsyncOperation.Post(
				() => _notifyIcon.ShowBalloonTip(timeout, tipTitle, tipText, GetToolTipIcon(notificationType)));
		}

		public string Tooltip
		{
			get { return _uiAsyncOperation.Send(() => _notifyIcon.Text); }
			set { _uiAsyncOperation.Post(() => _notifyIcon.Text = value); }
		}

		public Icon Icon
		{
			get{return _uiAsyncOperation.Send(() => _notifyIcon.Icon);}
			set{_uiAsyncOperation.Post(() => _notifyIcon.Icon = value);}
		}

		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				_uiAsyncOperation.Post(() => _trayMenuStrip.Enabled = _enabled);
			}
		}

		public bool Visible
		{
			get{return _uiAsyncOperation.Send(() => _notifyIcon.Visible);}
			set{_uiAsyncOperation.Post(() => _notifyIcon.Visible = value);}
		}

		public string ContextMenuName
		{
			get { return _contextMenuName; }
			set
			{
				if (_contextMenuName == value)
					return;

				_contextMenuName = value;

				if (_trayMenuGenerator != null)
				{
					_uiAsyncOperation.Send(() => _trayMenuGenerator.Dispose());
					_trayMenuGenerator = null;
				}
				if (_contextMenuName == null && _trayMenuStrip != null)
				{
					_uiAsyncOperation.Send(() => _trayMenuStrip.Dispose());
					_trayMenuStrip = null;
				}

				if (_contextMenuName != null)
				{
					if (_trayMenuStrip == null)
						_uiAsyncOperation.Send(
							() =>
							{
								_trayMenuStrip = new ContextMenuStrip();
								_notifyIcon.ContextMenuStrip = _trayMenuStrip;
								Ticker.Instance.ContextMenuStrip = _trayMenuStrip;
							});
					_trayMenuGenerator = new StripMenuGenerator(_serviceManager, _trayMenuStrip, _contextMenuName);
				}
			}
		}

		public void SetDefaultCommand(string commandName, IDictionary<string, object> parameters)
		{
			_defaultCommandService.SetDefaultCommand(commandName, parameters);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			ContextMenuName = null;
			_uiAsyncOperation.Send(() => _notifyIcon.Dispose());
			_uiAsyncOperation.OperationCompleted();
		}

		#endregion

		#region Private Members

		private void NotifyIconDefaultAction(object sender, EventArgs e)
		{
			if (_enabled)
				_serviceManager.ExecuteDefaultCommand();
		}

		private static ToolTipIcon GetToolTipIcon(NotificationType notificationType)
		{
			switch (notificationType)
			{
				case NotificationType.Info:
					return ToolTipIcon.Info;
				case NotificationType.Warning:
					return ToolTipIcon.Warning;
				case NotificationType.Error:
					return ToolTipIcon.Error;
				default:
					return ToolTipIcon.None;
			}
		} 
		
		#endregion
	}
}