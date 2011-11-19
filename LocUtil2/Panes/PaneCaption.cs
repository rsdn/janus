using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Summary description for PaneCaption.
	/// 
	/// Ported by Andre from VB and adapted
	/// </summary>
	[DesignTimeVisible(true)]
	public partial class PaneCaption : Panel
	{
		#region Declarations

		private const int DefaultHeight = 18;
		private const int DefaultFontSize = 9;
		private const int PosOffset = 4;
		private const string DefaultFontName = "arial";

		private bool _active;
		private bool _antiAlias = true;
		private bool _allowActive = true;

		private Color _colorActiveText = Color.Black;
		private Color _colorInactiveText = Color.White;

		private Color _colorActiveLow = Color.FromArgb(255, 165, 78);
		private Color _colorActiveHigh = Color.FromArgb(255, 225, 155);
		private Color _colorInactiveLow = Color.FromArgb(3, 55, 145);
		private Color _colorInactiveHigh = Color.FromArgb(90, 135, 215);

		// gdi objects
		private SolidBrush _brushActiveText;
		private SolidBrush _brushInactiveText;
		private LinearGradientBrush _brushActive;
		private LinearGradientBrush _brushInactive;

		private StringFormat _format;

		#endregion

		#region Constructor(s) & Dispose

		/// <summary>
		/// 
		/// </summary>
		public PaneCaption()
		{
			InitializeComponent();
			CustomInitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_brushActiveText != null)
					_brushActiveText.Dispose();
				if (_brushInactiveText != null)
					_brushInactiveText.Dispose();
				if (_brushActive != null)
					_brushActive.Dispose();
				if (_brushInactive != null)
					_brushInactive.Dispose();
				if (_format != null)
					_format.Dispose();

				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Private Methods

		private void CustomInitializeComponent()
		{
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.UserPaint
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.ResizeRedraw, true);

			this.Height = DefaultHeight;

			_format = new StringFormat();
			_format.FormatFlags = StringFormatFlags.NoWrap;
			_format.LineAlignment = StringAlignment.Center;
			_format.Trimming = StringTrimming.EllipsisCharacter;

			this.Font = new Font(DefaultFontName, DefaultFontSize, FontStyle.Bold);

			// create gdi objects
			this.ActiveTextColor = _colorActiveText;
			this.InactiveTextColor = _colorInactiveText;

			// setting the height above actually does this, but leave
			// in incase change the code (and forget to init the 
			// gradient brushes)
			CreateGradientBrushes();
		}

		private void CreateGradientBrushes()
		{
			// can only create brushes when have a width and height
			if (this.Width > 0 && this.Height > 0)
			{
				if (_brushActive != null)
					_brushActive.Dispose();
				_brushActive = new LinearGradientBrush(this.DisplayRectangle,
													   _colorActiveHigh, _colorActiveLow, LinearGradientMode.Vertical);

				if (_brushInactive != null)
					_brushInactive.Dispose();
				_brushInactive = new LinearGradientBrush(this.DisplayRectangle,
														 _colorInactiveHigh, _colorInactiveLow, LinearGradientMode.Vertical);
			}
		}

		private void DrawCaption(Graphics g)
		{
			// background
			g.FillRectangle(this.BackBrush, this.DisplayRectangle);

			// caption
			if (_antiAlias)
				g.TextRenderingHint = TextRenderingHint.AntiAlias;

			// need a rectangle when want to use ellipsis
			RectangleF bounds = CtrlHelper.CheckedRectangleF(PosOffset, 0,
															 this.DisplayRectangle.Width - PosOffset, this.DisplayRectangle.Height);

			g.DrawString(this.Text, this.Font, this.TextBrush, bounds, _format);
		}

		#endregion

		#region Base overrides

		/// <summary>
		/// 
		/// </summary>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			CreateGradientBrushes();
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (this._allowActive)
				this.Focus();
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			DrawCaption(e.Graphics);
			base.OnPaint(e);
		}

		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public SolidBrush TextBrush
		{
			get
			{
				if (_active && _allowActive)
					return _brushActiveText;
				else
					return _brushInactiveText;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public LinearGradientBrush BackBrush
		{
			get
			{
				if (_active && _allowActive)
					return _brushActive;
				else
					return _brushInactive;
			}
		}

		#endregion

		#region Color Properties

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("Color of the text when active."),
			DefaultValue(typeof (Color), "Black")]
		public Color ActiveTextColor
		{
			get { return _colorActiveText; }
			set
			{
				if (value.Equals(Color.Empty))
					value = Color.Black;
				_colorActiveText = value;
				_brushActiveText = new SolidBrush(_colorActiveText);
				Invalidate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("Color of the text when inactive."),
			DefaultValue(typeof (Color), "White")]
		public Color InactiveTextColor
		{
			get { return _colorInactiveText; }
			set
			{
				if (value.Equals(Color.Empty))
					value = Color.White;
				_colorInactiveText = value;
				_brushInactiveText = new SolidBrush(_colorInactiveText);
				Invalidate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("Low color of the active gradient."),
			DefaultValue(typeof (Color), "255, 165, 78")]
		public Color ActiveGradientLowColor
		{
			get { return _colorActiveLow; }
			set
			{
				if (value.Equals(Color.Empty))
					value = Color.FromArgb(255, 165, 78);
				_colorActiveLow = value;
				CreateGradientBrushes();
				Invalidate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("High color of the active gradient."),
			DefaultValue(typeof (Color), "255, 225, 155")]
		public Color ActiveGradientHighColor
		{
			get { return _colorActiveHigh; }
			set
			{
				if (value.Equals(Color.Empty))
					value = Color.FromArgb(255, 225, 155);
				_colorActiveHigh = value;
				CreateGradientBrushes();
				Invalidate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("Low color of the inactive gradient."),
			DefaultValue(typeof (Color), "3, 55, 145")]
		public Color InactiveGradientLowColor
		{
			get { return _colorInactiveLow; }
			set
			{
				if (value.Equals(Color.Empty))
					value = Color.FromArgb(3, 55, 145);
				_colorInactiveLow = value;
				CreateGradientBrushes();
				Invalidate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("High color of the inactive gradient."),
			DefaultValue(typeof (Color), "90, 135, 215")]
		public Color InactiveGradientHighColor
		{
			get { return _colorInactiveHigh; }
			set
			{
				if (value.Equals(Color.Empty))
					value = Color.FromArgb(90, 135, 215);
				_colorInactiveHigh = value;
				CreateGradientBrushes();
				Invalidate();
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("Text that is displayed in the label."),
			Browsable(true),
			DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get { return base.Text; }
			set
			{
				base.Text = value;
				Invalidate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("The active state of the caption, draws the caption with different gradient colors."),
			DefaultValue(false)]
		public bool Active
		{
			get { return _active; }
			set
			{
				_active = value;
				Invalidate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Appearance"),
			Description("If should draw the text as antialiased."),
			DefaultValue(true)]
		public bool AntiAlias
		{
			get { return _antiAlias; }
			set
			{
				_antiAlias = value;
				Invalidate();
			}
		}

		#endregion
	}
}