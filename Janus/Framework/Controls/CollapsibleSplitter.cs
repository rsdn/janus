using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Rsdn.Janus.Framework
{

	#region Enums
	/// <summary>
	/// Enumeration to sepcify the visual style to be applied to the CollapsibleSplitter control
	/// </summary>
	public enum VisualStyle
	{
		Mozilla = 0,
		XP,
		Win9X,
		DoubleDots,
		Lines
	}

	/// <summary>
	/// Enumeration to specify the current animation state of the control.
	/// </summary>
	public enum SplitterState
	{
		Collapsed = 0,
		Expanding,
		Expanded,
		Collapsing
	}
	#endregion

	/// <summary>
	/// A custom collapsible splitter that can resize, hide and show associated form controls
	/// </summary>
	[Designer(typeof (CollapsibleSplitterDesigner))]
	public class CollapsibleSplitter : Splitter
	{
		#region Private Properties
		// declare and define some base properties
		private readonly Timer _animationTimer;
		private readonly Color _hotColor = CalculateColor(SystemColors.Highlight, SystemColors.Window, 70);
		private int _animationDelay = 20;
		private int _animationStep = 20;

		// Border added in version 1.3
		private Border3DStyle _borderStyle = Border3DStyle.Flat;

		// animation controls introduced in version 1.22
		private int _controlHeight;
		private Control _controlToHide;
		private int _controlWidth;
		private SplitterState _currentState;
		private bool _expandParentForm;
		private bool _hot;
		private Form _parentForm;
		private int _parentFormHeight;
		private int _parentFormWidth;
		private Rectangle _rr;
		private bool _useAnimations;
		private VisualStyle _visualStyle;
		#endregion

		#region Public Properties
		/// <summary>
		/// The initial state of the Splitter. Set to True if the control to hide is not visible by default
		/// </summary>
		[Bindable(true)]
		[Category("Collapsing Options")]
		[DefaultValue("False")]
		[Description(
			"The initial state of the Splitter. Set to True if the control to hide is not visible by default"
			)]
		public bool IsCollapsed
		{
			get
			{
				if (_controlToHide != null)
					return !_controlToHide.Visible;
				return true;
			}
		}

		/// <summary>
		/// The System.Windows.Forms.Control that the splitter will collapse
		/// </summary>
		[Bindable(true)]
		[Category("Collapsing Options")]
		[DefaultValue("")]
		[Description("The System.Windows.Forms.Control that the splitter will collapse")]
		public Control ControlToHide
		{
			get { return _controlToHide; }
			set { _controlToHide = value; }
		}

		/// <summary>
		/// Determines if the collapse and expanding actions will be animated
		/// </summary>
		[Bindable(true)]
		[Category("Collapsing Options")]
		[DefaultValue("True")]
		[Description("Determines if the collapse and expanding actions will be animated")]
		public bool UseAnimations
		{
			get { return _useAnimations; }
			set { _useAnimations = value; }
		}

		/// <summary>
		/// The delay in millisenconds between animation steps
		/// </summary>
		[Bindable(true)]
		[Category("Collapsing Options")]
		[DefaultValue("20")]
		[Description("The delay in millisenconds between animation steps")]
		public int AnimationDelay
		{
			get { return _animationDelay; }
			set { _animationDelay = value; }
		}

		/// <summary>
		/// The amount of pixels moved in each animation step
		/// </summary>
		[Bindable(true)]
		[Category("Collapsing Options")]
		[DefaultValue("20")]
		[Description("The amount of pixels moved in each animation step")]
		public int AnimationStep
		{
			get { return _animationStep; }
			set { _animationStep = value; }
		}

		/// <summary>
		/// When true the entire parent form will be expanded and collapsed, otherwise just the contol to expand will be changed
		/// </summary>
		[Bindable(true)]
		[Category("Collapsing Options")]
		[DefaultValue("False")]
		[Description(
			"When true the entire parent form will be expanded and collapsed, otherwise just the contol to expand will be changed"
			)]
		public bool ExpandParentForm
		{
			get { return _expandParentForm; }
			set { _expandParentForm = value; }
		}

		/// <summary>
		/// The visual style that will be painted on the control
		/// </summary>
		[Bindable(true)]
		[Category("Collapsing Options")]
		[DefaultValue("VisualStyles.XP")]
		[Description("The visual style that will be painted on the control")]
		public VisualStyle VisualStyle
		{
			get { return _visualStyle; }
			set
			{
				_visualStyle = value;
				Invalidate();
			}
		}

		/// <summary>
		/// An optional border style to paint on the control. Set to Flat for no border
		/// </summary>
		[Bindable(true)]
		[Category("Collapsing Options")]
		[DefaultValue("System.Windows.Forms.Border3DStyle.Flat")]
		[Description("An optional border style to paint on the control. Set to Flat for no border")]
		public Border3DStyle BorderStyle3D
		{
			get { return _borderStyle; }
			set
			{
				_borderStyle = value;
				Invalidate();
			}
		}
		#endregion

		#region Public Methods
		public void ToggleState()
		{
			ToggleSplitter();
		}
		#endregion

		#region Constructor
		public CollapsibleSplitter()
		{
			// Register mouse events
			Click += OnClick;
			Resize += OnResize;
			MouseLeave += OnMouseLeave;
			MouseMove += OnMouseMove;

			// Setup the animation timer control
			_animationTimer = new Timer {Interval = _animationDelay};
			_animationTimer.Tick += AnimationTimerTick;
		}
		#endregion

		#region Overrides
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			_parentForm = FindForm();

			// set the current state
			if (_controlToHide != null)
				_currentState = _controlToHide.Visible ? SplitterState.Expanded : SplitterState.Collapsed;
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}
		#endregion

		#region Event Handlers
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// if the hider control isn't hot, let the base resize action occur
			if (_controlToHide != null)
				if (!_hot && _controlToHide.Visible)
					base.OnMouseDown(e);
		}

		private void OnResize(object sender, EventArgs e)
		{
			Invalidate();
		}

		// this method was updated in version 1.11 to fix a flickering problem
		// discovered by John O'Byrne
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			// check to see if the mouse cursor position is within the bounds of our control
			if (e.X >= _rr.X && e.X <= _rr.X + _rr.Width && e.Y >= _rr.Y && e.Y <= _rr.Y + _rr.Height)
			{
				if (!_hot)
				{
					_hot = true;
					Cursor = Cursors.Hand;
					Invalidate();
				}
			}
			else
			{
				if (_hot)
				{
					_hot = false;
					Invalidate();
				}

				Cursor = Cursors.Default;

				if (_controlToHide != null)
					if (!_controlToHide.Visible)
						Cursor = Cursors.Default;
					else // Changed in v1.2 to support Horizontal Splitters
						if (Dock == DockStyle.Left || Dock == DockStyle.Right)
							Cursor = Cursors.VSplit;
						else
							Cursor = Cursors.HSplit;
			}
		}

		private void OnMouseLeave(object sender, EventArgs e)
		{
			// ensure that the hot state is removed
			_hot = false;
			Invalidate();
		}

		private void OnClick(object sender, EventArgs e)
		{
			if (_controlToHide != null && _hot &&
				_currentState != SplitterState.Collapsing &&
					_currentState != SplitterState.Expanding)
				ToggleSplitter();
		}

		private void ToggleSplitter()
		{
			// if an animation is currently in progress for this control, drop out
			if (_currentState == SplitterState.Collapsing || _currentState == SplitterState.Expanding)
				return;

			_controlWidth = _controlToHide.Width;
			_controlHeight = _controlToHide.Height;

			if (_controlToHide.Visible)
				if (_useAnimations)
				{
					_currentState = SplitterState.Collapsing;

					if (_parentForm != null)
						if (Dock == DockStyle.Left || Dock == DockStyle.Right)
							_parentFormWidth = _parentForm.Width - _controlWidth;
						else
							_parentFormHeight = _parentForm.Height - _controlHeight;

					_animationTimer.Enabled = true;
				}
				else
				{
					// no animations, so just toggle the visible state
					_currentState = SplitterState.Collapsed;
					_controlToHide.Visible = false;
					if (_expandParentForm && _parentForm != null)
						if (Dock == DockStyle.Left || Dock == DockStyle.Right)
							_parentForm.Width -= _controlToHide.Width;
						else
							_parentForm.Height -= _controlToHide.Height;
				}
			else
				// control to hide is collapsed
				if (_useAnimations)
				{
					_currentState = SplitterState.Expanding;

					if (Dock == DockStyle.Left || Dock == DockStyle.Right)
					{
						if (_parentForm != null)
							_parentFormWidth = _parentForm.Width + _controlWidth;
						_controlToHide.Width = 0;
					}
					else
					{
						if (_parentForm != null)
							_parentFormHeight = _parentForm.Height + _controlHeight;
						_controlToHide.Height = 0;
					}
					_controlToHide.Visible = true;
					_animationTimer.Enabled = true;
				}
				else
				{
					// no animations, so just toggle the visible state
					_currentState = SplitterState.Expanded;
					_controlToHide.Visible = true;
					if (_expandParentForm && _parentForm != null)
						if (Dock == DockStyle.Left || Dock == DockStyle.Right)
							_parentForm.Width += _controlToHide.Width;
						else
							_parentForm.Height += _controlToHide.Height;
				}
		}
		#endregion

		#region Implementation

		#region Animation Timer Tick
		private void AnimationTimerTick(object sender, EventArgs e)
		{
			switch (_currentState)
			{
				case SplitterState.Collapsing:

					if (Dock == DockStyle.Left || Dock == DockStyle.Right)
						// vertical splitter
						if (_controlToHide.Width > _animationStep)
						{
							if (_expandParentForm && _parentForm.WindowState != FormWindowState.Maximized
								&& _parentForm != null)
								_parentForm.Width -= _animationStep;
							_controlToHide.Width -= _animationStep;
						}
						else
						{
							if (_expandParentForm && _parentForm.WindowState != FormWindowState.Maximized
								&& _parentForm != null)
								_parentForm.Width = _parentFormWidth;
							_controlToHide.Visible = false;
							_animationTimer.Enabled = false;
							_controlToHide.Width = _controlWidth;
							_currentState = SplitterState.Collapsed;
							Invalidate();
						}
					else
						// horizontal splitter
						if (_controlToHide.Height > _animationStep)
						{
							if (_expandParentForm && _parentForm.WindowState != FormWindowState.Maximized
								&& _parentForm != null)
								_parentForm.Height -= _animationStep;
							_controlToHide.Height -= _animationStep;
						}
						else
						{
							if (_expandParentForm && _parentForm.WindowState != FormWindowState.Maximized
								&& _parentForm != null)
								_parentForm.Height = _parentFormHeight;
							_controlToHide.Visible = false;
							_animationTimer.Enabled = false;
							_controlToHide.Height = _controlHeight;
							_currentState = SplitterState.Collapsed;
							Invalidate();
						}
					break;

				case SplitterState.Expanding:

					if (Dock == DockStyle.Left || Dock == DockStyle.Right)
						// vertical splitter
						if (_controlToHide.Width < (_controlWidth - _animationStep))
						{
							if (_expandParentForm && _parentForm.WindowState != FormWindowState.Maximized
								&& _parentForm != null)
								_parentForm.Width += _animationStep;
							_controlToHide.Width += _animationStep;
						}
						else
						{
							if (_expandParentForm && _parentForm.WindowState != FormWindowState.Maximized
								&& _parentForm != null)
								_parentForm.Width = _parentFormWidth;
							_controlToHide.Width = _controlWidth;
							_controlToHide.Visible = true;
							_animationTimer.Enabled = false;
							_currentState = SplitterState.Expanded;
							Invalidate();
						}
					else
						// horizontal splitter
						if (_controlToHide.Height < (_controlHeight - _animationStep))
						{
							if (_expandParentForm && _parentForm.WindowState != FormWindowState.Maximized
								&& _parentForm != null)
								_parentForm.Height += _animationStep;
							_controlToHide.Height += _animationStep;
						}
						else
						{
							if (_expandParentForm && _parentForm.WindowState != FormWindowState.Maximized
								&& _parentForm != null)
								_parentForm.Height = _parentFormHeight;
							_controlToHide.Height = _controlHeight;
							_controlToHide.Visible = true;
							_animationTimer.Enabled = false;
							_currentState = SplitterState.Expanded;
							Invalidate();
						}
					break;
			}
		}
		#endregion

		#region Paint the control
		// OnPaint is now an override rather than an event in version 1.1
		protected override void OnPaint(PaintEventArgs e)
		{
			// create a Graphics object
			var g = e.Graphics;

			// find the rectangle for the splitter and paint it
			var r = ClientRectangle; // fixed in version 1.1
			g.FillRectangle(new SolidBrush(BackColor), r);

			#region Vertical Splitter
			// Check the docking style and create the control rectangle accordingly
			if (Dock == DockStyle.Left || Dock == DockStyle.Right)
			{
				// create a new rectangle in the vertical center of the splitter for our collapse control button
				_rr = new Rectangle(r.X, r.Y + ((r.Height - 115)/2), 8, 115);
				// force the width to 8px so that everything always draws correctly
				Width = 8;

				// draw the background color for our control image
				g.FillRectangle(
					_hot
						? new SolidBrush(_hotColor)
						: new SolidBrush(BackColor),
					new Rectangle(_rr.X + 1, _rr.Y, 6, 115));

				// draw the top & bottom lines for our control image
				g.DrawLine(new Pen(SystemColors.ControlDark, 1), _rr.X + 1, _rr.Y, _rr.X + _rr.Width - 2, _rr.Y);
				g.DrawLine(new Pen(SystemColors.ControlDark, 1), _rr.X + 1, _rr.Y + _rr.Height, _rr.X + _rr.Width - 2,
					_rr.Y + _rr.Height);

				if (Enabled)
				{
					// draw the arrows for our control image
					// the ArrowPointArray is a point array that defines an arrow shaped polygon
					g.FillPolygon(new SolidBrush(SystemColors.ControlDarkDark), ArrowPointArray(_rr.X + 2, _rr.Y + 3));
					g.FillPolygon(new SolidBrush(SystemColors.ControlDarkDark),
						ArrowPointArray(_rr.X + 2, _rr.Y + _rr.Height - 9));
				}

				// draw the dots for our control image using a loop
				var x = _rr.X + 3;
				var y = _rr.Y + 14;

				// Visual Styles added in version 1.1
				switch (_visualStyle)
				{
					case VisualStyle.Mozilla:

						for (var i = 0; i < 30; i++)
						{
							// light dot
							g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y + (i*3), x + 1, y + 1 + (i*3));
							// dark dot
							g.DrawLine(new Pen(SystemColors.ControlDarkDark), x + 1, y + 1 + (i*3), x + 2, y + 2 + (i*3));
							// overdraw the background color as we actually drew 2px diagonal lines, not just dots
							g.DrawLine(
								_hot ? new Pen(_hotColor) : new Pen(BackColor),
								x + 2,
								y + 1 + (i*3),
								x + 2,
								y + 2 + (i*3));
						}
						break;

					case VisualStyle.DoubleDots:
						for (var i = 0; i < 30; i++)
						{
							// light dot
							g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x, y + 1 + (i*3), 1, 1);
							// dark dot
							g.DrawRectangle(new Pen(SystemColors.ControlDark), x - 1, y + (i*3), 1, 1);
							i++;
							// light dot
							g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 2, y + 1 + (i*3), 1, 1);
							// dark dot
							g.DrawRectangle(new Pen(SystemColors.ControlDark), x + 1, y + (i*3), 1, 1);
						}
						break;

					case VisualStyle.Win9X:

						g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y, x + 2, y);
						g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y, x, y + 90);
						g.DrawLine(new Pen(SystemColors.ControlDark), x + 2, y, x + 2, y + 90);
						g.DrawLine(new Pen(SystemColors.ControlDark), x, y + 90, x + 2, y + 90);
						break;

					case VisualStyle.XP:

						for (var i = 0; i < 18; i++)
						{
							// light dot
							g.DrawRectangle(new Pen(SystemColors.ControlLight), x, y + (i*5), 2, 2);
							// light light dot
							g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 1, y + 1 + (i*5), 1, 1);
							// dark dark dot
							g.DrawRectangle(new Pen(SystemColors.ControlDarkDark), x, y + (i*5), 1, 1);
							// dark fill
							g.DrawLine(new Pen(SystemColors.ControlDark), x, y + (i*5), x, y + (i*5) + 1);
							g.DrawLine(new Pen(SystemColors.ControlDark), x, y + (i*5), x + 1, y + (i*5));
						}
						break;

					case VisualStyle.Lines:

						for (var i = 0; i < 44; i++)
							g.DrawLine(new Pen(SystemColors.ControlDark), x, y + (i*2), x + 2, y + (i*2));

						break;
				}

				// Added in version 1.3
				if (_borderStyle != Border3DStyle.Flat)
				{
					// Paint the control border
					ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, _borderStyle, Border3DSide.Left);
					ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, _borderStyle, Border3DSide.Right);
				}
			}

				#endregion

				// Horizontal Splitter support added in v1.2

				#region Horizontal Splitter
			else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
			{
				// create a new rectangle in the horizontal center of the splitter for our collapse control button
				_rr = new Rectangle(r.X + ((r.Width - 115)/2), r.Y, 115, 8);
				// force the height to 8px
				Height = 8;

				// draw the background color for our control image
				g.FillRectangle(
					_hot ? new SolidBrush(_hotColor) : new SolidBrush(BackColor),
					new Rectangle(_rr.X, _rr.Y + 1, 115, 6));

				// draw the left & right lines for our control image
				g.DrawLine(new Pen(SystemColors.ControlDark, 1), _rr.X, _rr.Y + 1, _rr.X, _rr.Y + _rr.Height - 2);
				g.DrawLine(new Pen(SystemColors.ControlDark, 1), _rr.X + _rr.Width, _rr.Y + 1, _rr.X + _rr.Width,
					_rr.Y + _rr.Height - 2);

				if (Enabled)
				{
					// draw the arrows for our control image
					// the ArrowPointArray is a point array that defines an arrow shaped polygon
					g.FillPolygon(new SolidBrush(SystemColors.ControlDarkDark), ArrowPointArray(_rr.X + 3, _rr.Y + 2));
					g.FillPolygon(new SolidBrush(SystemColors.ControlDarkDark),
						ArrowPointArray(_rr.X + _rr.Width - 9, _rr.Y + 2));
				}

				// draw the dots for our control image using a loop
				var x = _rr.X + 14;
				var y = _rr.Y + 3;

				// Visual Styles added in version 1.1
				switch (_visualStyle)
				{
					case VisualStyle.Mozilla:

						for (var i = 0; i < 30; i++)
						{
							// light dot
							g.DrawLine(new Pen(SystemColors.ControlLightLight), x + (i*3), y, x + 1 + (i*3), y + 1);
							// dark dot
							g.DrawLine(new Pen(SystemColors.ControlDarkDark), x + 1 + (i*3), y + 1, x + 2 + (i*3), y + 2);
							// overdraw the background color as we actually drew 2px diagonal lines, not just dots
							g.DrawLine(
								_hot ? new Pen(_hotColor) : new Pen(BackColor),
								x + 1 + (i*3), y + 2, x + 2 + (i*3), y + 2);
						}
						break;

					case VisualStyle.DoubleDots:

						for (var i = 0; i < 30; i++)
						{
							// light dot
							g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 1 + (i*3), y, 1, 1);
							// dark dot
							g.DrawRectangle(new Pen(SystemColors.ControlDark), x + (i*3), y - 1, 1, 1);
							i++;
							// light dot
							g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 1 + (i*3), y + 2, 1, 1);
							// dark dot
							g.DrawRectangle(new Pen(SystemColors.ControlDark), x + (i*3), y + 1, 1, 1);
						}
						break;

					case VisualStyle.Win9X:

						g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y, x, y + 2);
						g.DrawLine(new Pen(SystemColors.ControlLightLight), x, y, x + 88, y);
						g.DrawLine(new Pen(SystemColors.ControlDark), x, y + 2, x + 88, y + 2);
						g.DrawLine(new Pen(SystemColors.ControlDark), x + 88, y, x + 88, y + 2);
						break;

					case VisualStyle.XP:

						for (var i = 0; i < 18; i++)
						{
							// light dot
							g.DrawRectangle(new Pen(SystemColors.ControlLight), x + (i*5), y, 2, 2);
							// light light dot
							g.DrawRectangle(new Pen(SystemColors.ControlLightLight), x + 1 + (i*5), y + 1, 1, 1);
							// dark dark dot
							g.DrawRectangle(new Pen(SystemColors.ControlDarkDark), x + (i*5), y, 1, 1);
							// dark fill
							g.DrawLine(new Pen(SystemColors.ControlDark), x + (i*5), y, x + (i*5) + 1, y);
							g.DrawLine(new Pen(SystemColors.ControlDark), x + (i*5), y, x + (i*5), y + 1);
						}
						break;

					case VisualStyle.Lines:

						for (var i = 0; i < 44; i++)
							g.DrawLine(new Pen(SystemColors.ControlDark), x + (i*2), y, x + (i*2), y + 2);

						break;
				}

				// Added in version 1.3
				if (_borderStyle != Border3DStyle.Flat)
				{
					// Paint the control border
					ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, _borderStyle, Border3DSide.Top);
					ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, _borderStyle, Border3DSide.Bottom);
				}
			}

				#endregion

			else
				throw new Exception(
					"The Collapsible Splitter control cannot have the Filled or None Dockstyle property");

			// dispose the Graphics object
			g.Dispose();
		}
		#endregion

		#region Arrow Polygon Array
		// This creates a point array to draw a arrow-like polygon
		private Point[] ArrowPointArray(int x, int y)
		{
			var point = new Point[3];

			if (_controlToHide != null)
				// decide which direction the arrow will point
				if (
					(Dock == DockStyle.Right && _controlToHide.Visible)
						|| (Dock == DockStyle.Left && !_controlToHide.Visible)
					)
				{
					// right arrow
					point[0] = new Point(x, y);
					point[1] = new Point(x + 3, y + 3);
					point[2] = new Point(x, y + 6);
				}
				else if (
					(Dock == DockStyle.Right && !_controlToHide.Visible)
						|| (Dock == DockStyle.Left && _controlToHide.Visible)
					)
				{
					// left arrow
					point[0] = new Point(x + 3, y);
					point[1] = new Point(x, y + 3);
					point[2] = new Point(x + 3, y + 6);
				}

					// Up/Down arrows added in v1.2
				else if (
					(Dock == DockStyle.Top && _controlToHide.Visible)
						|| (Dock == DockStyle.Bottom && !_controlToHide.Visible)
					)
				{
					// up arrow
					point[0] = new Point(x + 3, y);
					point[1] = new Point(x + 6, y + 4);
					point[2] = new Point(x, y + 4);
				}
				else if (
					(Dock == DockStyle.Top && !_controlToHide.Visible)
						|| (Dock == DockStyle.Bottom && _controlToHide.Visible)
					)
				{
					// down arrow
					point[0] = new Point(x, y);
					point[1] = new Point(x + 6, y);
					point[2] = new Point(x + 3, y + 3);
				}

			return point;
		}
		#endregion

		#region Color Calculator
		// this method was borrowed from the RichUI Control library by Sajith M
		private static Color CalculateColor(Color front, Color back, int alpha)
		{
			// solid color obtained as a result of alpha-blending

			var frontColor = Color.FromArgb(255, front);
			var backColor = Color.FromArgb(255, back);

			float frontRed = frontColor.R;
			float frontGreen = frontColor.G;
			float frontBlue = frontColor.B;
			float backRed = backColor.R;
			float backGreen = backColor.G;
			float backBlue = backColor.B;

			var fRed = frontRed*alpha/255 + backRed*((float)(255 - alpha)/255);
			var newRed = (byte)fRed;
			var fGreen = frontGreen*alpha/255 + backGreen*((float)(255 - alpha)/255);
			var newGreen = (byte)fGreen;
			var fBlue = frontBlue*alpha/255 + backBlue*((float)(255 - alpha)/255);
			var newBlue = (byte)fBlue;

			return Color.FromArgb(255, newRed, newGreen, newBlue);
		}
		#endregion

		#endregion
	}

	/// <summary>
	/// A simple designer class for the CollapsibleSplitter control to remove 
	/// unwanted properties at design time.
	/// </summary>
	public class CollapsibleSplitterDesigner : ControlDesigner
	{
		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("IsCollapsed");
			properties.Remove("BorderStyle");
			properties.Remove("Size");
		}
	}
}