using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Круглый индикатор прогресса
	/// </summary>
	public class ProgressPieControl : Control
	{
		#region Constructor
		public ProgressPieControl()
		{
			SetStyle(ControlStyles.UserPaint
				| ControlStyles.AllPaintingInWmPaint
					| ControlStyles.OptimizedDoubleBuffer
						| ControlStyles.ResizeRedraw, true);

			ForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
		}
		#endregion

		#region Event
		/// <summary>
		/// Вызывается при изменении значения индикатора
		/// </summary>
		public event EventHandler ValueChanged;

		/// <summary>
		/// Вызывает событие ValueChanged
		/// </summary>
		/// <param name="e">аргументы события</param>
		protected void OnValueChanged(EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, e);
		}
		#endregion

		#region Fields & Properties
		private int _maximum = 100;
		private bool _onlyFullSectors = true;
		private Color _pieBackColor = SystemColors.ControlLightLight;

		private Color _pieIndicatorColor = SystemColors.ActiveCaption;

		private int _sectorCount = 20;
		private int _val;

		/// <summary>
		/// Цвет фона индикатора
		/// </summary>
		[DefaultValue(typeof (Color), "ControlLightLight")]
		public Color PieBackColor
		{
			get { return _pieBackColor; }
			set
			{
				_pieBackColor = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Цвет заливки индикатора
		/// </summary>
		[DefaultValue(typeof (Color), "ActiveCaption")]
		public Color PieIndicatorColor
		{
			get { return _pieIndicatorColor; }
			set
			{
				_pieIndicatorColor = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Количество секторов. Всегда четно
		/// </summary>
		[DefaultValue(20)]
		public int SectorCount
		{
			get { return _sectorCount; }
			set
			{
				_sectorCount = (value/2)*2;
				Invalidate();
			}
		}

		/// <summary>
		/// Значение, соответствующее 100%
		/// </summary>
		[DefaultValue(100)]
		public int Maximum
		{
			get { return _maximum; }
			set
			{
				_maximum = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Значение, отображаемое индикатором
		/// </summary>
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.All)]
		public int Value
		{
			get { return _val; }
			set
			{
				_val = value <= Maximum ? value : Maximum;
				OnValueChanged(EventArgs.Empty);
				Invalidate();
			}
		}

		/// <summary>
		/// Показывать только заполненные полностью сектора
		/// </summary>
		[DefaultValue(true)]
		public bool OnlyFullSectors
		{
			get { return _onlyFullSectors; }
			set
			{
				_onlyFullSectors = value;
				Invalidate();
			}
		}
		#endregion

		#region Overrides
		protected override Size DefaultSize
		{
			get { return new Size(80, 80); }
		}

		/// <summary>
		/// Текст в центре элемента управления
		/// </summary>
		[ReadOnly(true)]
		public override string Text
		{
			get
			{
				var percents = (int)((float)Value/Maximum*100);
				return percents + "%";
			}
			set
			{
				// Do nothing, Text property is read only now
			}
		}

		/// <summary>
		/// Цвет надписи
		/// </summary>
		[DefaultValue(typeof (Color), "ControlDarkDark")]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			DrawControl(e.Graphics);
		}
		#endregion

		#region Private methods
		private const int _indicatorSpaceSize = 2;

		private void DrawSectorLine(Graphics g, Pen p, double angle)
		{
			var csx = (double)Width/2*Math.Sin(angle);
			var csy = (double)Width/2*Math.Cos(angle);
			g.DrawLine(p,
				(float)(Width/2 - csx),
				(float)(Width/2 - csy),
				(float)(Width/2 + csx),
				(float)(Width/2 + csy));
		}

		private void DrawControl(Graphics g)
		{
			g.SmoothingMode = SmoothingMode.HighQuality;
			var pts = g.MeasureString("100%", Font);
			using (Brush br = new SolidBrush(PieBackColor))
			{
				g.FillEllipse(br, 0, 0, Width - 1, Width - 1);
				var ea = OnlyFullSectors
					?
						(int)Math.Round((double)Value/Maximum*SectorCount)*(360.0/SectorCount)
					: (double)Value/Maximum*360.0;
				g.FillPie(new SolidBrush(PieIndicatorColor),
					_indicatorSpaceSize,
					_indicatorSpaceSize,
					Width - 1 - _indicatorSpaceSize*2,
					Width - 1 - _indicatorSpaceSize*2,
					-90, (float)ea);

				var wc = (int)(Width/2 - pts.Width/2);
				var ws = (int)(pts.Width);
				g.FillEllipse(br,
					wc, wc, ws, ws);
			}

			using (var spPen = new Pen(PieBackColor, _indicatorSpaceSize))
				for (var i = 0; i < SectorCount/2; i++)
					DrawSectorLine(g, spPen, (float)Math.PI/SectorCount*2*i);

			g.DrawEllipse(new Pen(ForeColor), 0, 0, Width - 1, Width - 1);

			using (var sf = new StringFormat())
			{
				sf.Alignment = StringAlignment.Center;
				using (Brush br = new SolidBrush(ForeColor))
					g.DrawString(Text, Font, br,
						Width/2, Width/2 - (int)(pts.Height/2), sf);
			}
		}
		#endregion
	}
}