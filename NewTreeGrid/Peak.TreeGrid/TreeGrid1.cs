using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Peak.TreeGrid.Properties;
using System.Drawing.Drawing2D;

namespace Peak.TreeGrid
{
	[ToolboxItem(false)]
	public partial class TreeGrid1 : UserControl
	{
		public TreeGrid1()
		{
			InitializeComponent();
			columns = new ColumnCollection(this);

			realGrid.LostFocus += new EventHandler(realGrid_LostFocus);
			realGrid.GotFocus += new EventHandler(realGrid_GotFocus);
		}

		ColumnCollection columns;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ColumnCollection Columns
		{
			get { return columns; }
		}

		public event EventHandler ColumnsChanged;

		internal protected virtual void OnColumnsChanged(EventArgs e)
		{
			if (ColumnsChanged != null)
				ColumnsChanged(this, e);
			realGrid.Columns.Clear();
			realGrid.Columns.AddRange(columns.ConvertAll(
				column => GenerateGridColumn(column)).ToArray());
		}

		protected virtual DataGridViewColumn GenerateGridColumn(Column column)
		{
			DataGridViewColumn ret = new DataGridViewTextBoxColumn();
			FillGridColumn(column, ret);
			return ret;
		}

		internal static void FillGridColumn(Column column, DataGridViewColumn ret)
		{
			ret.HeaderText = column.HeadingText;
			ret.DataPropertyName = column.DataPropertyName;
			ret.Width = column.Width;
		}

		[DefaultValue(25)]
		[DisplayName("Header height")]
		public int HeaderHeight
		{
			get { return realGrid.ColumnHeadersHeight; }
			set { realGrid.ColumnHeadersHeight = value; }
		}

		protected virtual VisualInfo DoGetVisualInfo(int row, int column)
		{
			DataGridViewCell cell = realGrid.Rows[row].Cells[column];
			return new VisualInfo(row % 2 == 1, realGrid.SelectedCells.Contains(
				cell), realGrid.CurrentCell == cell, Focused, Enabled);

		}

		protected virtual CellInfo DoGetCellInfo(int row, int column)
		{
			return new CellInfo(string.Format("{0} : {1}", row + 1, column + 1),
				null);
		}

		protected virtual StyleInfo DoGetStyleInfo(int row, int column)
		{
			return null;
		}

		protected virtual int DoGetIdent(int row, int column)
		{
			return 0;
		}

		protected virtual bool? DoGetExpanded(int row, int column)
		{
			return null;
		}

		protected virtual bool DoDrawExpansion(int column)
		{
			return true;
		}

		protected virtual void DoExpandCollapse(int row)
		{
		}

		#region Focus got/lost
		void realGrid_GotFocus(object sender, EventArgs e)
		{
			OnGotFocus(e);
		}

		void realGrid_LostFocus(object sender, EventArgs e)
		{
			OnLostFocus(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			realGrid.Invalidate();
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			realGrid.Invalidate();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			realGrid.Invalidate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			realGrid.Invalidate();
		}


		#endregion
		
		private const int expansionImageWidth = 16;

		private void realGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			Rectangle r = e.CellBounds;
			Graphics g = e.Graphics;
			int row = e.RowIndex;
			int column = e.ColumnIndex;
			DataGridViewCellStyle cellStyle = e.CellStyle;
			DataGridViewElementStates cellState = e.State;
			if (row >= 0 && column >= 0)
			{
				CellInfo cell = DoGetCellInfo(row, column);
				StyleInfo style = DoGetStyleInfo(row, column);
				if (style == null)
				{
					style = DoGenerateDefaultStyle(column, cellStyle, cellState);
				}
				int ident = DoGetIdent(row, column);
				r = DoDrawBackground(r, g, style);
				r.X += 2;
				r.Y += 1;
				r.Width -= 4;
				r.Height -= 2;
				if (DoDrawExpansion(column))
				{
					Rectangle r3 = new Rectangle(r.X + ident, r.Y, expansionImageWidth, r.Height);
					ident += expansionImageWidth;
					bool? isExpanded = DoGetExpanded(row, column);
					if (isExpanded != null)
					{
						if ((Application.VisualStyleState & VisualStyleState.ClientAreaEnabled) ==
							VisualStyleState.ClientAreaEnabled && VisualStyleRenderer.IsSupported)
						{
							VisualStyleRenderer rdr = new VisualStyleRenderer(
								(((bool)isExpanded) ?
								VisualStyleElement.TreeView.Glyph.Opened :
								VisualStyleElement.TreeView.Glyph.Closed));
							rdr.DrawBackground(g, r3);
						}
						else
						{
							Image img = (((bool)isExpanded) ? Resources.Collapse : Resources.Expand);
							if (img.Height < r3.Height)
							{
								int chg = r3.Height - img.Height;
								r3.Y += chg / 2;
								r3.Height = img.Height;
							}
							g.DrawImage(img, r3);
						}
					}
				}
				r.X += ident;
				r.Width -= ident;
				DrawCellContents(g, cell, style, r);
				e.Handled = true;
			}
			else if (row == -1 && column >= 0)
			{
				CellInfo cell = new CellInfo(Columns[column].HeadingText,
					Columns[column].Image);
				StyleInfo style = new StyleInfo(Color.Transparent, e.CellStyle.ForeColor,
					e.CellStyle.Font, Columns[column].HeadingAlignment);
				e.PaintBackground(e.ClipBounds, false);
				r.X += 2;
				r.Y += 1;
				r.Width -= 4;
				r.Height -= 2;
				DrawCellContents(g, cell, style, r);
				e.Handled = true;
			}
		}

		private Color gridColor = SystemColors.ControlDark;

		public Color GridColor
		{
			get { return gridColor; }
			set { gridColor = value; }
		}

		private bool ShouldSerializeGridColor()
		{
			return gridColor != SystemColors.ControlDark;
		}

		protected virtual Rectangle DoDrawBackground(Rectangle r, Graphics g, StyleInfo style)
		{
			using (Brush bgBrush = new LinearGradientBrush(r.Location, new Point(r.X, r.Bottom),
				style.BackColor, style.BackColor2))
				g.FillRectangle(bgBrush, r);
			Rectangle r2 = new Rectangle(r.Location, r.Size - new Size(1, 1));
			using (Pen edgePen = new Pen(GridColor))
			{
				g.DrawLine(edgePen, new Point(r2.Left, r2.Bottom), new Point(r2.Right, r2.Bottom));
				g.DrawLine(edgePen, new Point(r2.Right, r2.Top), new Point(r2.Right, r2.Bottom));
			}
			return r;
		}

		protected virtual StyleInfo DoGenerateDefaultStyle(int column, DataGridViewCellStyle cellStyle, DataGridViewElementStates cellState)
		{
			StyleInfo style = null;
			if ((cellState & DataGridViewElementStates.Selected) != DataGridViewElementStates.Selected)
			{
				style = new StyleInfo(cellStyle.BackColor, cellStyle.ForeColor,
					cellStyle.Font, Columns[column].Alignment,
					ChangeColor(cellStyle.BackColor, -32), 
					ChangeColor(cellStyle.ForeColor, 32));
			}
			else
			{
				if (realGrid.Focused && Enabled)
				{
					style = new StyleInfo(SystemColors.ActiveCaption,
						SystemColors.ActiveCaptionText,
						cellStyle.Font, Columns[column].Alignment,
						SystemColors.GradientActiveCaption,
						SystemColors.ActiveCaptionText);
				}
				else
				{
					style = new StyleInfo(SystemColors.InactiveCaption, SystemColors.InactiveCaptionText,
						cellStyle.Font, Columns[column].Alignment,
					   SystemColors.GradientInactiveCaption,
					   SystemColors.InactiveCaptionText);
				}
			}
			return style;
		}

		private static Color ChangeColor(Color color, int p)
		{
			int changeR = (int)((float)color.R / 255.0f * p);
			int changeG = (int)((float)color.G / 255.0f * p);
			int changeB = (int)((float)color.B / 255.0f * p);
			int newR = Fix255(color.R + changeR);
			int newG = Fix255(color.G + changeG);
			int newB = Fix255(color.B + changeB);
			return Color.FromArgb(newR, newG, newB);
		}

		private static int Fix255(int p)
		{
			if (p < 0)
				return 0;
			else if (p > 255)
				return 255;
			else
				return p;
		}

		private Rectangle DrawCellContents(Graphics g, CellInfo cell, StyleInfo style, Rectangle r)
		{
			using (Brush foreBrush = new LinearGradientBrush(r.Location, new Point(r.X, r.Bottom), 
				style.ForeColor, style.ForeColor2))
			{
				if (cell.Image != null)
				{
					Rectangle r2 = new Rectangle(r.X + 1, r.Y + 1, cell.Image.Width, cell.Image.Height);
					if (r2.Height > r.Height)
					{
						r2.Height = r.Height;
					}
					else if (r2.Height < r.Height)
					{
						r2.Y += ((r.Height - r2.Height) >> 1);
					}
					g.DrawImage(cell.Image, r2);
					r.X += cell.Image.Width + 2;
					r.Width -= (cell.Image.Width + 2);
				}
				r.X += 1;
				r.Y += 1;
				r.Width -= 1;
				r.Height -= 1;
				StringFormat sf = new StringFormat();
				sf.Alignment = HorAlToStrAl(style.Algnment);
				sf.LineAlignment = StringAlignment.Center;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				g.DrawString(cell.Text, Font, foreBrush, r, sf);
			}
			return r;
		}

		#region Utility funcs
		static StringAlignment HorAlToStrAl(HorizontalAlignment Alignment)
		{
			switch (Alignment)
			{
				case HorizontalAlignment.Left:
					return StringAlignment.Near;
				case HorizontalAlignment.Right:
					return StringAlignment.Far;
				default:
				case HorizontalAlignment.Center:
					return StringAlignment.Center;
			}
		} 
		#endregion

		protected virtual void realGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (DoDrawExpansion(e.ColumnIndex))
			{
				int idnt = DoGetIdent(e.RowIndex, e.ColumnIndex);

				Rectangle r = realGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
				r.X += (idnt);
				r.Width = expansionImageWidth;
				r.Y = 0;
				if (r.Contains(e.Location))
				{
					DoExpandCollapse(e.RowIndex);
				}
			}
		}
	}
}
