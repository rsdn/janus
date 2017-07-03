using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Services;
using CodeJam.Strings;

using JetBrains.Annotations;

using Rsdn.Janus.ObjectModel;
using Rsdn.Shortcuts;
using Rsdn.Janus.Framework;

using WeifenLuo.WinFormsUI.Docking;

using NativeMethods = Rsdn.Janus.Framework.NativeMethods;

namespace Rsdn.Janus
{
	public partial class MainForm : JanusBaseForm
	{
		#region Declarations

		private const int _idSyncSysMenu = 0x2462;

		private readonly IServiceProvider _serviceProvider;
		private NavigationComboBoxGenerator _navigationComboBoxGenerator;
		private StripMenuGenerator _mainMenuGenerator;
		private StripMenuGenerator _toolBarGenerator;

		#endregion

		#region Constructor(s) & Dispose

		public MainForm([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			var serviceManager = new ServiceContainer(serviceProvider);

			Features.Instance.Init();
			var activeMsgSvc = new FeatureActiveMessageService();
			serviceManager.Publish<IActiveMessagesService>(activeMsgSvc);

			_serviceProvider = serviceManager;

			InitializeComponent();
			CustomInitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (Config.Instance.TickerConfig.ShowTicker)
				Ticker.HideTicker();

			if (disposing)
			{
				_navigationComboBoxGenerator.Dispose();
				if (_mainMenuGenerator != null)
				{
					_mainMenuGenerator.Dispose();
					_mainMenuGenerator = null;
				}
				if (_toolBarGenerator != null)
				{
					_toolBarGenerator.Dispose();
					_toolBarGenerator = null;
				}

				StyleConfig.StyleChange -= OnStyleChanged;

				components?.Dispose();
			}

			base.Dispose(disposing);

			Config.Save();
		}

		#endregion

		#region Static methods

		internal static string GetCaption()
		{
			return
				"{0} - {1} ({2})".FormatWith(
					CultureInfo.InvariantCulture,
					ApplicationInfo.ApplicationName,
					Config.Instance.Login);
		}

		#endregion

		#region Overrides

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			SystemMenuHelper.AddItemToMenu(
				Handle.ToInt32(),
				_idSyncSysMenu,
				SR.MainForm.SystemMenuHelper.SyncItemCaption,
				true);

			_navigationComboBoxGenerator = new NavigationComboBoxGenerator(_serviceProvider, _navigationBox);
			_mainMenuGenerator = new StripMenuGenerator(_serviceProvider, _menuStrip, "MainForm.Menu");
			_toolBarGenerator = new StripMenuGenerator(_serviceProvider, _toolStrip, "MainForm.Toolbar");
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (_serviceProvider.GetRequiredService<ISynchronizer>().IsActive())
			{
				if (
					MessageBox.Show(
						this,
						SR.MainForm.SyncActiveWarning,
						SR.MainForm.AppCloseTitle,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning) != DialogResult.Yes)
				{
					e.Cancel = true;
					return;
				}
			}

			e.Cancel =
				Config.Instance.ConfirmationConfig.ConfirmClosing
				&& MessageBox.Show(
					this,
					SR.MainForm.CloseQuestion.FormatWith(ApplicationInfo.ApplicationName),
					SR.MainForm.AppCloseTitle,
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Question) != DialogResult.OK;
		}

		protected override void OnClosed(EventArgs ea)
		{
			base.OnClosed(ea);

			Config.Instance.MainFormBounds.Bounds =
				WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;

			// ---> Обработка максимизации формы (by Andir)--->
			Config.Instance.MainFormBounds.Maximized = WindowState == FormWindowState.Maximized;

			// this is to avoid ugly flicking at exit
			WindowState = FormWindowState.Minimized;

			Config.Instance.XmlShortcuts = _shortcutManager.SaveToXmlNode();

			// Запись конфига была перенесена на Dispose, так как теперь 
			// GUI-контролы отображающие интерфейс для фич сохраняют свое 
			// состояние непосредствено внутри себя.
			Features.Instance.Dispose();
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);

			if (WindowState == FormWindowState.Minimized && Config.Instance.MinimizeToTray)
				Hide();
		}

		protected override void WndProc(ref Message msg)
		{
			switch (msg.Msg)
			{
				case NativeMethods.WM_SYSCOMMAND:
					{
						base.WndProc(ref msg);
						if (msg.WParam.ToInt32() == _idSyncSysMenu)
							_serviceProvider.TryExecuteCommand(
								"Janus.Synchronization.Synchronize",
								new Dictionary<string, object>());
					}
					break;
				default:
					base.WndProc(ref msg);
					break;
			}
		}

		#endregion

		#region Поддержка стилей

		private void OnStyleChanged(object s, StyleChangeEventArgs e)
		{
			UpdateStyle();
			Refresh();
		}

		private void UpdateStyle()
		{
			_navigationBox.BackColor = StyleConfig.Instance.NavigationTreeBack;
			_navigationBox.Font = StyleConfig.Instance.NavigationTreeFont;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Панель докинга.
		/// </summary>
		internal DockPanel DockPanel { get; private set; }

		internal ShortcutManager ShortcutManager => _shortcutManager;
		#endregion

		#region Private methods

		private void CustomInitializeComponent()
		{
			Text = GetCaption();

			Bounds = Config.Instance.MainFormBounds.Bounds;
			if (Config.Instance.MainFormBounds.Maximized)
				WindowState = FormWindowState.Maximized;

			var userShortcuts = Config.Instance.XmlShortcuts;
			if (userShortcuts != null)
				_shortcutManager.LoadFromXmlNode(userShortcuts);

			ConfigureUI();

			// Стили
			StyleConfig.StyleChange += OnStyleChanged;
			UpdateStyle();

			if (Config.Instance.TickerConfig.ShowTicker)
				Ticker.ShowTicker(_serviceProvider);

			Application.AddMessageFilter(new WheelDispatcher());
			Config.Instance.ConfigChanged += (sender, e) => ConfigureUI();
		}

		private void ConfigureUI()
		{
			_navigationBox.Visible = Config.Instance.NavigationComboConfig.Show;
			_navigationBox.MaxDropDownItems = Config.Instance.NavigationComboConfig.MaxDropDownItems;
		}

		#endregion

		#region Shortcuts

		[MethodShortcut(Shortcut.None, "О программе", "Информация о программе.")]
		private void About()
		{
			_serviceProvider.TryExecuteCommand("Janus.Application.About", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Выход", "Выход из программы.")]
		private void ExitApp()
		{
			_serviceProvider.TryExecuteCommand("Janus.Application.Exit", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.F5, "Синхронизация",
				"Синхронизация с сервером для принятия или отправки сообщений.")]
		private void Sync()
		{
			_serviceProvider.TryExecuteCommand("Janus.Synchronization.Synchronize", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.F9, "Подписка", "Подписка на форумы.")]
		private void Subscribe()
		{
			_serviceProvider.TryExecuteCommand("Janus.Forum.Subscription", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.F6, "Следующий форум", "Перейти на следующий форум.")]
		private void SelectNextFeature()
		{
			_serviceProvider.TryExecuteCommand("Janus.Navigation.SelectNextFeature", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.ShiftF6, "Предыдущий форум", "Вернуться на предыдущий форум.")]
		private void SelectPreviousFeature()
		{
			_serviceProvider.TryExecuteCommand("Janus.Navigation.SelectPreviousFeature", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.None, "Настройка приложения", "Настройка приложения.")]
		private void Options()
		{
			_serviceProvider.TryExecuteCommand("Janus.Application.AppOptions", new Dictionary<string, object>());
		}

		[MethodShortcut(Shortcut.F11, "Настройки пользователя", "Настройки пользователя.")]
		private void UserPrefs()
		{
			_serviceProvider.TryExecuteCommand("Janus.Application.UserOptions", new Dictionary<string, object>());
		}

		#endregion
	}
}
