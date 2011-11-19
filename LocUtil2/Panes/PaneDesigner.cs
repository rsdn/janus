using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Summary description for PaneDesigner.
	/// 
	/// Ported by Andre from VB and adapted
	/// </summary>
	public class PaneDesigner : ParentControlDesigner
	{
		private Pen _borderPen = new Pen(SystemColors.ControlDark);
		private Pen _workspacePen = new Pen(SystemColors.ControlLight);

		private Pane paneControl;

		/// <summary>
		/// 
		/// </summary>
		public PaneDesigner()
			: base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			paneControl = Control as Pane;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);
			pe.Graphics.DrawRectangle(BorderPen, 0, 0, paneControl.Width - 1, paneControl.Height - 1);
			pe.Graphics.DrawRectangle(WorkspacePen, paneControl.WorkspaceRect);
		}

		/// <summary>
		/// 
		/// </summary>
		public Pen BorderPen
		{
			get { return _borderPen; }
			set { _borderPen = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public Pen WorkspacePen
		{
			get { return _workspacePen; }
			set { _workspacePen = value; }
		}
	}
}