using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{

	#region Border style enum
	/// <summary>
	/// Стиль цвета границы панели
	/// </summary>
	public enum FramePanelBorderStyle
	{
		/// <summary>
		/// Серая рамка
		/// </summary>
		Grey,

		/// <summary>
		/// Цвет текущей подсветки (ala Office2003)
		/// </summary>
		System
	}
	#endregion

	/// <summary>
	/// Панель с границей
	/// </summary>
	public class FramePanel : Panel
	{
		#region Constructor(s)
		public FramePanel()
		{
			SetStyle(ControlStyles.UserPaint
				| ControlStyles.AllPaintingInWmPaint
					| ControlStyles.OptimizedDoubleBuffer
						| ControlStyles.ResizeRedraw, true);

			InitializeDockPadding();
		}
		#endregion

		#region Fields & Properties
		private FramePanelBorderStyle _panelBorderStyle = FramePanelBorderStyle.Grey;

		[Browsable(true)]
		[DefaultValue(FramePanelBorderStyle.Grey)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public FramePanelBorderStyle PanelBorderStyle
		{
			get { return _panelBorderStyle; }
			set { _panelBorderStyle = value; }
		}
		#endregion

		#region Private Methods
		private void InitializeDockPadding()
		{
			if (DockPadding.Left == 0)
				DockPadding.Left = 1;
			if (DockPadding.Right == 0)
				DockPadding.Right = 1;
			if (DockPadding.Top == 0)
				DockPadding.Top = 1;
			if (DockPadding.Bottom == 0)
				DockPadding.Bottom = 1;
		}
		#endregion

		#region Overrides
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var x = 0 + (DockPadding.Left - 1);
			var y = (0 + DockPadding.Top - 1);
			var width = Width - (x + (DockPadding.Right - 1));
			var height = Height - (y + (DockPadding.Bottom - 1));

			var borderColor = _panelBorderStyle == FramePanelBorderStyle.Grey
				? SystemColors.ControlDark
				: SystemColors.Highlight;

			ControlPaint.DrawBorder(e.Graphics,
				new Rectangle(x, y, width, height),
				borderColor, ButtonBorderStyle.Solid);
		}
		#endregion
	}
}