using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Внутренний браузер.
	/// </summary>
	internal sealed partial class WebBrowserForm : JanusBaseForm, IMessageFilter
	{
		//ToDo: close by escape shortcut
		//ToDo: mouse back shortcut
		//ToDo: mouse forward shortcut

		private readonly ServiceManager _serviceManager;
		private readonly AsyncOperation _asyncOp;
		private readonly StripMenuGenerator _menuGenerator;
		private readonly StripMenuGenerator _toolbarGenerator;

		public WebBrowserForm(IServiceProvider serviceProvider)
		{
			_serviceManager = new ServiceManager(serviceProvider);
			_asyncOp = AsyncHelper.CreateOperation();

			InitializeComponent();

			_serviceManager.Publish<IBrowserFormService>(new JBrowserFormService(this));
			_serviceManager.Publish<IActiveMessagesService>(
				new JBrowserActiveMessageService(_serviceManager));

			//Восстановление размера и положеня формы
			Bounds = Config.Instance.WebBrowserFormBounds.Bounds;
			if (Config.Instance.WebBrowserFormBounds.Maximized)
				WindowState = FormWindowState.Maximized;

			_menuGenerator = new StripMenuGenerator(_serviceManager, _menuStrip, "JBrowser.Menu");
			_toolbarGenerator = new StripMenuGenerator(_serviceManager, _toolStrip, "JBrowser.Toolbar");

			_webBrowser.BackColor = Color.White;

			//Большинство событий браузера в дизайнере не видно, поэтому добавляем вручную
			_webBrowser.ProgressChanged += WebBrowserProgressChanged;
			_webBrowser.Navigated += WebBrowserNavigated;
			_webBrowser.StatusTextChanged += WebBrowserStatusTextChanged;
			_webBrowser.DocumentTitleChanged += WebBrowserDocumentTitleChanged;
			_webBrowser.CanGoBackChanged += WebBrowserCanGoBackChanged;
			_webBrowser.CanGoForwardChanged += WebBrowserCanGoForwardChanged;
			_webBrowser.DocumentCompleted += WebBrowserDocCompleted;

			var styleImageManager = _serviceManager.GetService<IStyleImageManager>();
			if (styleImageManager != null)
			{
				var image = styleImageManager.TryGetImage("jbrowser", StyleImageType.Large);
				if (image != null) 
					Icon = image.ToIcon();
			}
		}

		#region Public Members

		public string Url
		{
			get
			{
				var uri = _asyncOp.Send(() => _webBrowser.Url);
				return uri != null ? uri.ToString() : string.Empty;
			}
		}

		public void NavigateForward()
		{
			_asyncOp.Post(() => _webBrowser.GoForward());
		}

		public void NavigateBackward()
		{
			_asyncOp.Post(() => _webBrowser.GoBack());
		}

		public bool CanNavigateForward
		{
			get
			{
				return _asyncOp.Send(() => _webBrowser.CanGoForward);
			}
		}

		public bool CanNavigateBackward
		{
			get
			{
				return _asyncOp.Send(() => _webBrowser.CanGoBack);
			}
		}

		public bool CanStop
		{
			get
			{
				return _asyncOp.Send(() => _webBrowser.IsBusy);
			}
		}

		public void NavigateTo(string url)
		{
			_asyncOp.Post(() => _webBrowser.Navigate(url));
		}

		public void RefreshPage()
		{
			_asyncOp.Post(() => _webBrowser.Refresh(WebBrowserRefreshOption.Normal));
		}

		public void Stop()
		{
			_asyncOp.Post(() => _webBrowser.Stop());
		}

		public new void Close()
		{
			_asyncOp.Post(() => base.Close());
		}

		public event EventHandler Navigated;

		public event EventHandler CanNavigateBackwardChanged;

		public event EventHandler CanNavigateForwardChanged;

		public event EventHandler DocumentCompleted;

		#endregion

		#region Protected Members

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_webBrowser.ProgressChanged -= WebBrowserProgressChanged;
				_webBrowser.Navigated -= WebBrowserNavigated;
				_webBrowser.StatusTextChanged -= WebBrowserStatusTextChanged;
				_webBrowser.DocumentTitleChanged -= WebBrowserDocumentTitleChanged;
				_webBrowser.CanGoBackChanged -= WebBrowserCanGoBackChanged;
				_webBrowser.CanGoForwardChanged -= WebBrowserCanGoForwardChanged;

				_menuGenerator.Dispose();
				_toolbarGenerator.Dispose();

				_asyncOp.OperationCompleted();

				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Application.AddMessageFilter(this);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Config.Instance.WebBrowserFormBounds.Bounds =
				WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;
			Config.Instance.WebBrowserFormBounds.Maximized =
				WindowState == FormWindowState.Maximized;

			Application.RemoveMessageFilter(this);
		}

		private void OnNavigated(EventArgs e)
		{
			var navigatedHandler = Navigated;
			if (navigatedHandler != null)
				navigatedHandler(this, e);
		}

		private void OnCanNavigateBackwardChanged(EventArgs e)
		{
			if (CanNavigateBackwardChanged != null)
				CanNavigateBackwardChanged(this, e);
		}

		private void OnCanNavigateForwardChanged(EventArgs e)
		{
			if (CanNavigateForwardChanged != null)
				CanNavigateForwardChanged(this, e);
		}
		#endregion

		#region Escape key processing

		// Кнопки браузером перехватываются, поэтому стандартный способ обработки
		// сообщений не работает, приходится вешать хук.
		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			// ReSharper disable InconsistentNaming
			const int WM_KEYDOWN = 0x0100;
			const int VK_ESCAPE = 0x1B;
			// ReSharper restore InconsistentNaming

			if (ActiveForm == this && m.Msg == WM_KEYDOWN && (int)m.WParam == VK_ESCAPE)
			{
				Close();
				return true;
			}

			return false;
		}

		#endregion

		#region Controls events

		private void _locationComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (_locationComboBox.SelectedIndex > -1)
				_webBrowser.Navigate(_locationComboBox.Items[_locationComboBox.SelectedIndex].ToString());
		}

		private void _locationComboBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				_webBrowser.Navigate(_locationComboBox.Text);
				e.Handled = true;
			}
		}

		private void WebBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			var navigatedUri = _webBrowser.Url.ToString();

			var itemIndex = _locationComboBox.Items.IndexOf(navigatedUri);

			if (itemIndex > -1)
				_locationComboBox.Items.RemoveAt(itemIndex);

			_locationComboBox.Items.Insert(0, navigatedUri);

			if (_locationComboBox.Items.Count > 50)
				_locationComboBox.Items.RemoveAt(
					_locationComboBox.Items.Count - 1);

			_locationComboBox.SelectedIndex = 0;

			OnNavigated(EventArgs.Empty);
		}

		private void WebBrowserDocumentTitleChanged(object sender, EventArgs e)
		{
			Text = String.IsNullOrEmpty(_webBrowser.DocumentTitle) ?
				"JBrowser" : _webBrowser.DocumentTitle + " - JBrowser";
		}

		private void WebBrowserStatusTextChanged(object sender, EventArgs e)
		{
			_stateStatusLabel.Text = _webBrowser.StatusText;
		}

		private void WebBrowserProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
		{
			if (e.CurrentProgress == -1L)
				return;

			if (e.CurrentProgress == 0L)
				_statusProgressBar.Visible = false;
			else
			{
				_statusProgressBar.Visible = true;
				_statusProgressBar.Maximum = (int)e.MaximumProgress;
				var currentProgress = e.CurrentProgress;
				if (currentProgress > e.MaximumProgress)
					currentProgress = e.MaximumProgress;
				_statusProgressBar.Value = (int)currentProgress;
			}
		}

		private void WebBrowserCanGoForwardChanged(object sender, EventArgs e)
		{
			OnCanNavigateForwardChanged(EventArgs.Empty);
		}

		private void WebBrowserCanGoBackChanged(object sender, EventArgs e)
		{
			OnCanNavigateBackwardChanged(EventArgs.Empty);
		}

		private void WebBrowserDocCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			if (DocumentCompleted != null)
				DocumentCompleted(this, EventArgs.Empty);
		}
		#endregion
	}
}