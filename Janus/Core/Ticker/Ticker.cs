using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

using JetBrains.Annotations;

using WeifenLuo.WinFormsUI.Docking.Win32;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Полупрозрачное окошко с информацией.
	/// </summary>
	public class Ticker : Form, ITicker
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IForumsAggregatesService _forumsAggregatesService;
		private readonly IDisposable _aggregatesChangedSubscription;
		private readonly List<ITickerIndicator> _indicators = new List<ITickerIndicator>();
		private readonly TextIndicator _syncIndicator = new TextIndicator();
		private readonly TextIndicator _unreadMsgIndicator = new TextIndicator();
		private readonly Image _backImage;
		private int _fixX;
		private int _fixY;
		private int _initFixX;
		private int _initFixY;
		private static bool _tickerVisible;
		private static IDisposable _startSyncSubscription;
		private static IDisposable _endSyncSubscription;
		private static Ticker _instance;
		private const int _stickySpace = 12;

		public static Ticker Instance
		{
			get
			{
				return _instance
					?? (_instance = new Ticker(ApplicationManager.Instance.ServiceProvider));
			}
		}

		private Color BorderColor { get; set; }

		private Ticker([NotNull] IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			_serviceProvider = provider;
			_forumsAggregatesService = _serviceProvider.GetRequiredService<IForumsAggregatesService>();
			_aggregatesChangedSubscription =
				_forumsAggregatesService.AggregatesChanged.Subscribe(arg => UpdateUnread());

			BorderColor = Color.White;
			//			_janusApplication = ApplicationManager.Instance;
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Size = new Size(160, 32);
			TopMost = true;
			ShowInTaskbar = false;

			StartPosition = FormStartPosition.Manual;

			SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
			_backImage = new Bitmap(
				Assembly
					.GetAssembly(GetType())
					.GetRequiredResourceStream("Rsdn.Janus.Core.Ticker.tickerbg.png"));

			BackColor = Color.FromArgb(255, 0, 255);
			TransparencyKey = BackColor;

			CustomInitializeComponents();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_aggregatesChangedSubscription.Dispose();

			base.Dispose(disposing);
		}

		private void CustomInitializeComponents()
		{
			_indicators.Add(_syncIndicator);
			_syncIndicator.Owner = this;
			_syncIndicator.Text = "СИНХ";
			_syncIndicator.Font = new Font("Arial", 6, FontStyle.Bold);
			_syncIndicator.ForeColor = Color.FromArgb(0, 255, 80);
			_syncIndicator.BackColor = Color.Transparent;
			_syncIndicator.Bounds = new Rectangle(3, 3, 27, 8);

			_indicators.Add(_unreadMsgIndicator);
			_unreadMsgIndicator.Owner = this;
			_unreadMsgIndicator.Text = "000";
			_unreadMsgIndicator.Font = new Font("Arial", 6, FontStyle.Bold);
			_unreadMsgIndicator.ForeColor = Color.FromArgb(255, 255, 0);
			_unreadMsgIndicator.BackColor = Color.Transparent;
			_unreadMsgIndicator.OnImage = new Bitmap(
				Assembly
				.GetAssembly(GetType())
				.GetRequiredResourceStream("Rsdn.Janus.Core.Ticker.urmsgson.png"));
			_unreadMsgIndicator.OffImage = new Bitmap(
				Assembly
					.GetAssembly(GetType())
					.GetRequiredResourceStream("Rsdn.Janus.Core.Ticker.urmsgsoff.png"));
			//unreadMsgIndicator.On = true;
			_unreadMsgIndicator.Bounds = new Rectangle(33, 3, 60, 8);

			UpdateUnread();
		}

		private void DrawBackground(Graphics g)
		{
			g.DrawImage(_backImage, 0, 0, _backImage.Width, _backImage.Height);
		}

		private static void DrawIndicator(Graphics g, ITickerIndicator ind)
		{
			using (var rg = new Region(ind.Bounds))
			{
				g.Clip = rg;
				g.TranslateTransform(ind.Location.X, ind.Location.Y,
									 MatrixOrder.Append);
				ind.Draw(g);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			DrawBackground(e.Graphics);
			foreach (var ind in _indicators)
				using (var g = CreateGraphics())
					DrawIndicator(g, ind);
		}

		public void RedrawIndicator(ITickerIndicator ind)
		{
			using (var g = CreateGraphics())
			using (var cr = new Region(ind.Bounds))
			{
				g.Clip = cr;
				DrawBackground(g);
				DrawIndicator(g, ind);
			}
		}

		protected override void OnMove(EventArgs e)
		{
			if (Config.Instance.TickerConfig.StickyTicker)
			{
				var wa = Screen.GetWorkingArea(Instance);
				var loc = Location;
				loc.Offset(-wa.X, -wa.Y);
				var dx = Math.Abs(wa.Width - loc.X - Width);
				var dy = Math.Abs(wa.Height - loc.Y - Height);
				if (dx < _stickySpace)
				{
					_fixX = _fixX - (wa.Width - 1 - Width - loc.X);
					loc = new Point(wa.Width - 1 - Width, loc.Y);
				}
				else if (Math.Abs(loc.X) < _stickySpace)
				{
					_fixX = _fixX + loc.X;
					loc = new Point(0, loc.Y);
				}
				if ((dy < _stickySpace))
				{
					_fixY = _fixY - (wa.Height - 1 - Height - loc.Y);
					loc = new Point(loc.X, wa.Height - 1 - Height);
				}
				else if (Math.Abs(loc.Y) < _stickySpace)
				{
					_fixY = _fixY + loc.Y;
					loc = new Point(loc.X, 0);
				}
				loc.Offset(wa.X, wa.Y);
				Location = loc;
			}
			Config.Instance.TickerConfig.TickerPosition = Location;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_fixX = e.X;
				_fixY = e.Y;
				_initFixX = _fixX;
				_initFixY = _fixY;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (Config.Instance.TickerConfig.StickyTicker)
				{
					if (Math.Abs(_fixX - _initFixX) > _stickySpace)
						_fixX = _initFixX;
					if (Math.Abs(_fixY - _initFixY) > _stickySpace)
						_fixY = _initFixY;
				}
				var sp = PointToScreen(new Point(e.X, e.Y));
				Location = new Point(sp.X - _fixX, sp.Y - _fixY);
			}
		}

		private void UpdateUnread()
		{
			var forumsSvc = _serviceProvider.GetService<IForumsAggregatesService>();
			_unreadMsgIndicator.Text =
				forumsSvc != null
					? forumsSvc.UnreadMessagesCount + "/" + forumsSvc.UnreadRepliesToMeCount
					: string.Empty;
			_unreadMsgIndicator.On = forumsSvc != null && forumsSvc.UnreadMessagesCount > 0;
		}

		public static void ShowTicker(IServiceProvider provider)
		{
			if (_tickerVisible)
				return;

			var tickerConfig = Config.Instance.TickerConfig;

			if (tickerConfig.TickerPosition.X == TickerConfig.DefTickerValue
				&& tickerConfig.TickerPosition.Y == TickerConfig.DefTickerValue)
			{
				var wa = Screen.GetWorkingArea(Instance);
				Instance.Location = new Point(wa.Width - Instance.Width - 5,
					wa.Height - Instance.Height - 5);
			}
			else
				Instance.Location = tickerConfig.TickerPosition;

			Instance.Opacity = tickerConfig.TickerOpacity;

			var sz = provider.GetRequiredService<ISynchronizer>();
			_startSyncSubscription = sz.StartSync.Subscribe(arg => Instance._syncIndicator.On = true);
			_endSyncSubscription = sz.EndSync.Subscribe(arg => Instance._syncIndicator.On = false);

			Instance.Show();
			_tickerVisible = true;
		}

		public static void HideTicker()
		{
			if (!_tickerVisible)
				return;

			_startSyncSubscription.Dispose();
			_endSyncSubscription.Dispose();

			Instance.Hide();
			_tickerVisible = false;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			HideTicker();
			e.Cancel = false;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				var cp = base.CreateParams;
				cp.Style &= unchecked((int)~WindowStyles.WS_CAPTION);
				return cp;
			}
		}

	}
}