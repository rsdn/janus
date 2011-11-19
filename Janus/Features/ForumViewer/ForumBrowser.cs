using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Rsdn.Janus
{
	public class ForumBrowser :
		AxWebBrowser, IDocHostUIHandler, IWebBrowserEvents
	{

		public ForumBrowser()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_cDoc != null)
					_cDoc.SetUIHandler(null);
			}
			base.Dispose(disposing);
		}

		public void Init()
		{
			object o = new object();
			Navigate("about:blank", ref o, ref o, ref o, ref o);
			while (ReadyState != tagREADYSTATE.READYSTATE_COMPLETE)
				System.Windows.Forms.Application.DoEvents();
			SetDocUIHandler();
		}

		public void OnBrowseToWindow(object sender, DWebBrowserEvents2_NewWindow2Event e)
		{
			// e.ppDisp = this; // FB: Финт ушами :) не получается: "оно" не хочет навигировать в тоже окно
			e.cancel = true;
		}

		// Нужен, чтобы в Dispose сделать _cDoc.SetUIHandler(null),
		// поскольку к моменту вызова Dispose RCW может быть уже убит - 
		// взять свойство Document не получится.
		// А без SetUIHandler(null) эксплорер не освобождается
		private ICustomDoc _cDoc;

		public void SetDocUIHandler()
		{
			_cDoc = (ICustomDoc)Document;
			_cDoc.SetUIHandler(this);
		}

		#region IDocHostUIHandler implementation

		void IDocHostUIHandler.EnableModeless(int fEnable)
		{
		}

		void IDocHostUIHandler.FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
		{
			ppDORet = null;
		}

		void IDocHostUIHandler.GetDropTarget(IDropTarget pDropTarget, out IDropTarget ppDropTarget)
		{
			ppDropTarget = null;
		}

		void IDocHostUIHandler.GetExternal(out object ppDispatch)
		{
			ppDispatch = null;
		}

		void IDocHostUIHandler.GetHostInfo(ref _DOCHOSTUIINFO pInfo)
		{
			pInfo.dwFlags |= (uint)0x00000004; //DOCHOSTUIFLAG_NO3DBORDER   
			pInfo.dwFlags |= (uint)0x00040000; //DOCHOSTUIFLAG_THEME
		}

		void IDocHostUIHandler.GetOptionKeyPath(out string pchKey, uint dw)
		{
			pchKey = null;
		}

		void IDocHostUIHandler.HideUI()
		{
		}

		void IDocHostUIHandler.OnDocWindowActivate(int fActivate)
		{
		}

		void IDocHostUIHandler.OnFrameWindowActivate(int fActivate)
		{
		}

		void IDocHostUIHandler.ResizeBorder(ref MsHtmHstInterop.tagRECT prcBorder, IOleInPlaceUIWindow pUIWindow, int fRameWindow)
		{
		}

		void IDocHostUIHandler.ShowContextMenu(uint dwID, ref MsHtmHstInterop.tagPOINT ppt, object pcmdtReserved, object pdispReserved)
		{
			if (!Restricted)
				Marshal.ThrowExceptionForHR(-1); //	Разрешить выполнение

			if (dwID == 4 || dwID == 5)
				Marshal.ThrowExceptionForHR(-1); // Разрешить выполнение
		}

		void IDocHostUIHandler.ShowUI(uint dwID, IOleInPlaceActiveObject pActiveObject, IOleCommandTarget pCommandTarget, IOleInPlaceFrame pFrame, IOleInPlaceUIWindow pDoc)
		{
		}

		void IDocHostUIHandler.TranslateAccelerator(ref MsHtmHstInterop.tagMSG lpmsg, ref Guid pguidCmdGroup, uint nCmdID)
		{
			if (!Restricted)
				Marshal.ThrowExceptionForHR(-1); //	Разрешить выполнение

			const int WM_KEYDOWN = 0x0100;

			if (lpmsg.message == WM_KEYDOWN)
				Marshal.ThrowExceptionForHR(-1); // Разрешить выполнение

		}

		void IDocHostUIHandler.TranslateUrl(uint dwTranslate, ref ushort pchURLIn, IntPtr ppchURLOut)
		{
		}

		void IDocHostUIHandler.UpdateUI()
		{
		}

		#endregion

		//	FB:
		//	Поддержка отлова евента	WebBrowser::BeforeNavigate
		//		Изветстно, что DWebBrowserEvents2_BeforeNavigate2 никогда не
		//		доходит	до подписчика. Некоторые считают это багом IE.
		//		Поэтому	мы в ручную	сами подписываемся на WebBrowser::BeforeNavigate
		//		(без двоек).
		//
		//	Это	евент на который сможет	подписаться... кто захочет
		//
		public event BeforeNavigateEventHandler BeforeNavigate;

		private IWebBrowser control;

		private ConnectionPointCookie cookie;

		protected override void CreateSink()
		{
			try
			{
				cookie = new ConnectionPointCookie(control, this, typeof (IWebBrowserEvents));
				base.CreateSink();
			}
			catch
			{
			}
		}

		protected override void DetachSink()
		{
			try
			{
				cookie.Disconnect();
				base.DetachSink();
			}
			catch
			{
			}
		}

		protected override void AttachInterfaces()
		{
			try
			{
				control = (IWebBrowser)GetOcx();
				base.AttachInterfaces();
			}
			catch
			{
			}
		}

		#region Properties

		private bool restricted = false;

		/// <summary>
		/// FB: Так	как	ForumBrowser используется теперь не	только для просмотра сообщений,
		/// но и просто	как	браузер	(для www.rsdn.ru, например), то	я ввел это
		/// свойство, оно задет, надо ли ограничевать функциональность IE.
		/// </summary>
		[Browsable(true)]
		public bool Restricted
		{
			get { return restricted; }
			set { restricted = value; }
		}

		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Content
		{
			get
			{
				if (OcxState != null && HtmlDoc != null && HtmlDoc.body != null)
					return HtmlDoc.body.innerHTML;
				else
					return null;
			}
			set
			{
				if (OcxState != null)
				{
					// Release old body
					Marshal.ReleaseComObject(HtmlDoc.body);

					if (value != null && value.Length > 0)
					{
						HtmlDoc.body.innerHTML = value;
					}
					else
						HtmlDoc.body.innerHTML = " ";
				}
				//object o = null;
				//Navigate("about:"+value, ref o, ref o, ref o, ref o);
			}
		}


		private IHTMLDocument2 htmlDoc;

		private IHTMLDocument2 HtmlDoc
		{
			get
			{
				if (htmlDoc != null)
					Marshal.ReleaseComObject(htmlDoc);
				htmlDoc = Document as IHTMLDocument2;
				return htmlDoc;
			}
		}

		private bool _forwardEnabled;

		public bool ForwardEnabled
		{
			get { return _forwardEnabled; }
			set { _forwardEnabled = value; }
		}

		private bool _backEnabled;

		public bool BackEnabled
		{
			get { return _backEnabled; }
			set { _backEnabled = value; }
		}

		[Browsable(true)]
		public override Color BackColor
		{
			get
			{
				//TODO: Port to .Net 2.0
				return Color.White;
				//return Color.FromName((string)HtmlDoc.body.style.backgroundColor);
			}
			set
			{
				if (HtmlDoc != null)
					HtmlDoc.body.style.backgroundColor = value.Name;
			}
		}

		#endregion

		public void Navigate(string url)
		{
			object o = Missing.Value;
			Navigate(url, ref o, ref o, ref o, ref o);
		}

		public void RaiseBeforeNavigate(
			string url,
			int flags,
			string targetFrameName,
			ref object postData,
			string headers,
			ref bool cancel)
		{
			if (String.Compare(url, "about:blank", true) == 0)
				cancel = false;
			else
			{
				BeforeNavigateEventArgs e = new BeforeNavigateEventArgs(url, false);
				if (BeforeNavigate != null)
					BeforeNavigate(this, e);
				cancel = e.Cancel;
			}
		}

	}

	public delegate void BeforeNavigateEventHandler(
		object sender, BeforeNavigateEventArgs e);


	public class BeforeNavigateEventArgs
	{
		public BeforeNavigateEventArgs(string url, bool cancel)
		{
			this.url = url;
			this.cancel = cancel;
		}

		protected string url;

		public string Url
		{
			get { return url; }
		}

		protected bool cancel;

		public bool Cancel
		{
			get { return cancel; }
			set { cancel = value; }
		}
	}

	// 
	[
		Guid("eab22ac2-30c1-11cf-a7eb-0000c05bae0b"),
			InterfaceType(ComInterfaceType.InterfaceIsIDispatch)
		]
	public interface IWebBrowserEvents
	{
		[DispId(100)]
		void RaiseBeforeNavigate(
			string url,
			int flags,
			string targetFrameName,
			ref object postData,
			string headers,
			ref bool cancel);
	}

	//	FB:
	//	Это	просто шоб було.
	//
	[
		Guid("eab22ac1-30c1-11cf-a7eb-0000c05bae0b")
		]
	public interface IWebBrowser
	{
	}
}