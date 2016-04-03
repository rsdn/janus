using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using CodeJam.Threading;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for SplashForm.
	/// </summary>
	public partial class SplashForm : Form, IBootTimeInformer, IProgressService, IProgressVisualizer
	{
		private const int _moduleImageSize = 32;
		private const int _moduleItemWidth = 64;
		private const int _moduleTopShift = 8;
		private const int _moduleBottomShift = 8;
		private const int _moduleItemHeight = 80 - _moduleTopShift - _moduleBottomShift;
		private const int _moduleInterval = 8;

		private readonly List<ExtensionData> _extData = new List<ExtensionData>();
		private readonly AsyncOperation _asyncOp;
		private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

		public SplashForm(IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			// Эта инициализация должна быть только внутри конструктора,
			// чтобы отработал базовый конструктор Control и тинициализировал
			// винформсный контекст синхронизации
			_asyncOp = AsyncHelper.CreateOperation();

			InitializeComponent();

			Text = ApplicationInfo.ApplicationName;
			_versionLabel.Text = ApplicationInfo.FullVersion;
			_copyrightLabel.Text = ApplicationInfo.Copyright;
			_itemsBox.Font = new Font("Arial", 6);

			/*var extSvc = serviceProvider.GetRequiredService<IJanusExtensionManager>();
			foreach (var prv in extSvc.GetInfoProviders())
				_extData.Add(new ExtensionData(prv.GetDisplayName(), prv.GetIcon()));
			_extData.Sort((data1, data2) => data1.DisplayName.CompareTo(data2.DisplayName));*/
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
			base.OnPaint(e);
		}

		private void ModulesBoxPaint(object sender, PaintEventArgs e)
		{
			var x = _moduleInterval;
			using (_rwLock.GetReadLock())
				foreach (var data in _extData)
				{
					DrawExtension(data, e.Graphics, new Rectangle(x, _moduleTopShift,
						_moduleItemWidth, _moduleItemHeight));
					x += _moduleInterval + _moduleItemWidth;
				}
		}

		private void DrawExtension(ExtensionData data, Graphics graphics, Rectangle rectangle)
		{
			graphics.DrawImage(
				data.Icon,
				rectangle.Left + (rectangle.Width - _moduleImageSize) / 2,
				rectangle.Top,
				_moduleImageSize,
				_moduleImageSize);
			var sf = new StringFormat { Alignment = StringAlignment.Center };
			graphics.DrawString(
				data.DisplayName,
				_itemsBox.Font,
				Brushes.Black,
				new Rectangle(
					rectangle.Left,
					rectangle.Top + _moduleImageSize,
					rectangle.Width, rectangle.Height - _moduleImageSize),
				sf);
		}

		#region ExtensionData class
		private class ExtensionData
		{
			public ExtensionData(string displayName, Image icon)
			{
				DisplayName = displayName;
				Icon = icon;
			}

			public string DisplayName { get; }

			public Image Icon { get; }
		}
		#endregion

		#region IBootTimeInformer Members
		public void SetText(string text)
		{
			_asyncOp.Post(() => _statusLabel.Text = text);
		}

		public void AddItem(string itemText, Image itemIcon)
		{
			using (_rwLock.GetWriteLock())
				_extData.Add(new ExtensionData(itemText, itemIcon));
			_asyncOp.Post(_itemsBox.Invalidate);
		}
		#endregion

		#region IProgressService Members

		public IProgressVisualizer CreateVisualizer(bool allowCancel)
		{
			return this;
		}

		#endregion

		#region IProgressVisualizer Members

		public void SetProgressText(string text)
		{
			SetText(text);
		}

		public void ReportProgress(int total, int count) { }

		public void Complete() { }

		public bool CancelRequested => false;
		#endregion
	}
}
