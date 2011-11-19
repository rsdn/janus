using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Summary description for BasePane.
	/// 
	/// Ported by Andre from VB and adapted
	/// </summary>
	[Designer(typeof (PaneDesigner)), DesignTimeVisible(true)]
	public partial class Pane : Panel
	{
		#region Declaration

		/// <summary>
		/// 
		/// </summary>
		public delegate void OnPaneActive(object sender, EventArgs e);

		/// <summary>
		/// 
		/// </summary>
		public event OnPaneActive PaneActive;

		private PaneCaption _caption;

		#endregion

		#region Constructor(s) & Dispose

		/// <summary>
		/// 
		/// </summary>
		public Pane()
		{
			InitializeComponent();

			this.SetStyle(ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.UserPaint
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.ResizeRedraw
				| ControlStyles.SupportsTransparentBackColor
				| ControlStyles.ContainerControl, true);
		}

		#endregion

		#region Base Overrides
		/// <summary>
		/// See <see cref="Control.OnLayout"/>
		/// </summary>
		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			_caption.Width = Width - 2;
			Padding = new Padding(1, _caption.Height + 2, 1, 1);
			_caption.Top = 1;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(SystemPens.Highlight, new Rectangle(0, 0, Width - 1, Height - 1));

			base.OnPaint(e);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			_caption.Active = true;
			if (PaneActive != null)
				PaneActive(this, EventArgs.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);

			_caption.Active = false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ReadOnly(true)]
		//[TypeConverter(typeof (ExpandableObjectConverter))]
		public PaneCaption PaneCaptionContol
		{
			get { return _caption; }
		}

		private ArrayList _coll = new ArrayList();
		/// <summary>
		/// 
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ArrayList Coll
		{
			get { return _coll; }
		}

		/// <summary>
		/// For designer
		/// </summary>
		[Browsable(false)]
		public Rectangle WorkspaceRect
		{
			get { return CtrlHelper.CheckedRectangle(0, _caption.Height + 1, this.Width - 1, this.Height - _caption.Height - 2); }
		}

		#endregion
	}
}