using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Peak.TreeGrid
{
	public class StyleInfo
	{
		public Color BackColor { get; internal set; }

		public Color ForeColor { get; internal set; }

		public Color ForeColor2 { get; internal set; }

		public Color BackColor2 { get; internal set; }

		public Font Font { get; internal set; }

		public HorizontalAlignment Algnment { get; internal set; }

		public StyleInfo(Color BackColor, Color ForeColor, Font Font, HorizontalAlignment Alignment)
		{
			this.BackColor2 = this.BackColor = BackColor;
			this.ForeColor2 = this.ForeColor = ForeColor;
			this.Font = Font;
			this.Algnment = Alignment;
		}

		public StyleInfo(Color BackColor, Color ForeColor, Font Font, HorizontalAlignment Alignment, Color BackColor2, Color ForeColor2)
		{
			this.BackColor = BackColor;
			this.ForeColor = ForeColor;
			this.BackColor2 = BackColor2;
			this.ForeColor2 = ForeColor2;
			this.Font = Font;
			this.Algnment = Alignment;
		}
	}
}
