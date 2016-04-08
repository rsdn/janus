using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using CodeJam.Extensibility;
using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Контрол для просмотра сообщения.
	/// </summary>
	internal sealed partial class MsgViewer : UserControl
	{
		private IMsg _msg;
		private readonly ServiceContainer _serviceManager;
		private readonly StripMenuGenerator _toolbarGenerator;
		private bool _blockExternalNavigation;

		public MsgViewer(IServiceProvider provider)
		{
			_serviceManager = new ServiceContainer(provider);

			_serviceManager.Publish<IActiveMessagesService>(
				new MessageViewerActiveMessageService(this));

			this.AssignServices(_serviceManager);

			InitializeComponent();

			_messageBrowser.Navigating += MessageBrowserNavigating;
			_messageBrowser.StatusTextChanged += MessageBrowserStatusTextChanged;
#if DEBUG
			_messageBrowser.ScriptErrorsSuppressed = false;
#endif

			_toolbarGenerator = new StripMenuGenerator(_serviceManager, _toolStrip, "MessageViewer.Toolbar", true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_toolbarGenerator.Dispose();
				_messageBrowser.Navigating -= MessageBrowserNavigating;
				_messageBrowser.StatusTextChanged -= MessageBrowserStatusTextChanged;
			}

			base.Dispose(disposing);
		}

		private static string FormatURI(
			JanusProtocolResourceType resourceType, string parameters)
		{
			return JanusProtocolDispatcher.FormatURI(resourceType, parameters);
		}

		//private static string NationalUrlDecode(string url)
		//{
		//    var encoding = url.IndexOf("%D0") != -1
		//        ? Encoding.UTF8
		//        : Encoding.GetEncoding("windows-1251");

		//    return HttpUtility.UrlDecode(url, encoding);
		//}

		#region Public members

		public event EventHandler MsgChanged;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IMsg Msg
		{
			get { return _msg; }
			set
			{
				if (_msg == value)
					return;

				_msg = value;

				NavigateToUrl(
					_msg == null
						? "about:blank"
						: FormatURI(JanusProtocolResourceType.Message, _msg.ID.ToString()));

				OnMsgChanged();
			}
		}

		public void NavigateToUrl(string url)
		{
			_blockExternalNavigation = true;
			_messageBrowser.Navigate(url);
		}

		/// <summary>
		/// Прокручивает страницу с сообщением на лист вниз.
		/// </summary>
		/// <returns>True - если перелистнули страницу, иначе - false.</returns>
		public bool PageDown()
		{
			if (_messageBrowser.Document != null)
			{
				var msgBody = _messageBrowser.Document.GetElementById("MsgBody");

				if (msgBody == null)
					return false;

				var scrollTop = msgBody.ScrollTop;
				var height = msgBody.ClientRectangle.Height;
				var size = Config.Instance.SmartJumpPageSize;

				msgBody.ScrollTop += (int)((height / 100.0d) * size);

				// если позиция скроллбара не изменилась, 
				// значит достигнут конец.
				return scrollTop != msgBody.ScrollTop;
			}
			return false;
		}

		#endregion

		private void OnMsgChanged()
		{
			MsgChanged?.Invoke(this, EventArgs.Empty);
		}

		#region Controls events

		private void MessageBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			if (!_blockExternalNavigation)
			{
				var protocolInfo = JanusProtocolInfo.Parse(e.Url.ToString());
				var linkType = protocolInfo?.LinkType ?? LinkType.External;
				var obManager = _serviceManager.GetRequiredService<IOutboxManager>();
				var manager = ApplicationManager.Instance;

				switch (linkType)
				{
					case LinkType.Local:
						Debug.Assert(protocolInfo != null);
						if (protocolInfo.ResourceType == JanusProtocolResourceType.Message)
							manager.ForumNavigator.SelectMessage(protocolInfo.Id);
						else
							manager.ForumNavigator.SelectMessage(protocolInfo.Parameters);
						break;

					case LinkType.Absent:
						Debug.Assert(protocolInfo != null);
						obManager.AddTopicForDownloadWithConfirm(protocolInfo.Id);
						e.Cancel = true;
						break;

					case LinkType.External:
						_serviceManager.OpenUrlInBrowser(e.Url.OriginalString);
						e.Cancel = true;
						break;
				}
			}
			_blockExternalNavigation = false;
		}

		private void MessageBrowserStatusTextChanged(object sender, EventArgs e)
		{
			//Зачем так усложнять?
			//if (_messageBrowser.Url != null && _messageBrowser.Url.OriginalString == "about:blank")
			//    ClearStatusLabel();
			//else
			//    _statusLabel.Text = NationalUrlDecode(_messageBrowser.StatusText);
			_statusLabel.Text = _messageBrowser.StatusText;
		}

		#endregion
	}
}
