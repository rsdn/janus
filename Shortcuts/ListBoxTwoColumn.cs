using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rsdn.Shortcuts
{
	[ToolboxItem(false)]
	public class ListBoxShortcuts : ListBoxTwoColumn
	{
		#region  Class Variables
		//private const int M_ALT = 0x4000;
		//private const int M_CONTROL = 0x2000;
		//private const int M_SHIFT = 0x1000;
		private const int VK_ALT = 0x0012;
		//private const int VK_CONTROL = 0x0011;
		private const int VK_DOWN = 0x0028;
		private const int VK_LEFT = 0x0025;
		private const int VK_RIGHT = 0x0027;

		//private const int VK_SHIFT = 0x0010;
		private const int VK_SPACE = 0x0020;
		private const int VK_UP = 0x0026;

		private Keys _oldKeyPress;
		#endregion

		#region Overrides
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);

			if (e.Index < 0)
				return;

			var state = e.State;

			var rectFirst = e.Bounds;
			rectFirst.Width = WidthSplit;

			var rectSecond = e.Bounds;
			rectSecond.X = WidthSplit;

			rectFirst.Height -= 1;

			using (Brush br = new SolidBrush(e.BackColor))
				e.Graphics.FillRectangle(br, rectFirst);

			rectFirst.Height += 1;

			if (!DesignMode)
				using (Brush brFore = new SolidBrush(e.ForeColor))
				using (Brush brBlack = new SolidBrush(Color.Black))
				{
					//First column
					var sf = new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						FormatFlags = StringFormatFlags.LineLimit
					};

					var ts = (TextShortcut)Items[e.Index];

					var text = ts.ShortName != string.Empty
						? ts.ShortName
						: ts.MethodName;

					e.Graphics.DrawString(text, e.Font, brFore, rectFirst, sf);

					//Second column
					using (var br = state != DrawItemState.Selected ? brBlack : brFore)
					{
						var key = GetItemShortcut(e.Index);
						e.Graphics.DrawString(
							KeyToString(key), e.Font, br, rectSecond, sf);
					}
				}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			var iKey = (int)keyData;

			if (iKey == VK_LEFT || iKey == VK_UP ||
				iKey == VK_RIGHT || iKey == VK_DOWN ||
					Keys.Tab == keyData)
				return base.ProcessCmdKey(ref msg, keyData);

			var sc = (Shortcut)keyData;

			if ((int)keyData == VK_SPACE)
				sc = Shortcut.None;

			//((TextShortcut)this.Items[this.SelectedIndex]).Shortcut = sc;
			SetItemShortcut(SelectedIndex, sc);

			if (_oldKeyPress != keyData)
			{
				Invalidate(GetItemRectangle(SelectedIndex));
				_oldKeyPress = keyData;
			}

			return true;
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			var ch = ((int)_oldKeyPress) & 0xFF;

			if (ch < (VK_ALT + 1) && ch != 0 && ch != 13)
				SetNoneShortcut(SelectedIndex);

			if (e.Modifiers == 0 && e.KeyValue < (VK_ALT + 1))
				if (TestExistShortcut(_oldKeyPress, SelectedIndex))
				{
					SetNoneShortcut(SelectedIndex);
					MessageBox.Show("Такая комбинация уже сужествует!");
				}

			base.OnKeyUp(e);
		}
		#endregion

		#region Implementation
		private void SetNoneShortcut(int index)
		{
			SetItemShortcut(index, Shortcut.None);
			_oldKeyPress = Keys.None;

			Invalidate(GetItemRectangle(index));
		}

		private bool TestExistShortcut(Keys key, int index)
		{
			if (key == Keys.None)
				return false;

			var method = ((TextShortcut)Items[index]).MethodName;

			foreach (TextShortcut ts in Items)
				if (ts.Shortcut == (Shortcut)key && method != ts.MethodName)
					return true;

			return false;
		}

		private int GetItemShortcut(int index)
		{
			return (int)(((TextShortcut)Items[index]).Shortcut);
		}

		private void SetItemShortcut(int index, Shortcut shortcut)
		{
			((TextShortcut)Items[index]).Shortcut = shortcut;
		}

		private static string KeyToString(int key)
		{
			// удалим Keys.ControlKey, Keys.Menu, Keys.ShiftKey, так как они дублируются 
			// Keys.Control, Keys.Alt, Keys.Shift
			Keys keyCode = (Keys)key & Keys.KeyCode;
			if (keyCode == Keys.ControlKey || keyCode == Keys.Menu || keyCode == Keys.ShiftKey)
				key = key & (int)Keys.Modifiers;
			return new KeysConverter().ConvertToString(key);
		}
		#endregion
	}

	[ToolboxItem(false)]
	public class ListBoxTwoColumn : ListBox
	{
		#region  Class Variables
		//private const int nColumns = 2;
		private const int _offsetLines = 2;
		private const int _offsetSplit = 3;

		private bool _changeSize;
		private int _widthSplit;
		#endregion

		#region Constructors
		public ListBoxTwoColumn()
		{
			base.DrawMode = DrawMode.OwnerDrawFixed;
			base.ItemHeight += _offsetLines;
		}
		#endregion

		#region Properties
		public int WidthSplit
		{
			get { return _widthSplit; }
		}
		#endregion

		#region Overrides
		protected override void CreateHandle()
		{
			base.CreateHandle();
			_widthSplit = Width - (Width/4);
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0)
			{
				base.OnDrawItem(e);
				return;
			}

			if (!DesignMode)
				using (var pen = new Pen(Color.DarkGray))
				{
					e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);
					e.Graphics.DrawLine(pen, _widthSplit, e.Bounds.Y, _widthSplit,
						e.Bounds.Y + e.Bounds.Height - 1);
					e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1,
						e.Bounds.Width, e.Bounds.Y + e.Bounds.Height - 1);
				}
		}

		//		protected override void OnDrawItem(DrawItemEventArgs e)
		//		{
		//			if ( e.Index < 0)
		//			{
		//				base.OnDrawItem(e);
		//				return;
		//			}
		//
		//			DrawItemState state = e.State;
		//
		//			Rectangle rectFirst = e.Bounds;
		//			rectFirst.Width = _widthSplit;
		//
		//			Rectangle rectSecond = e.Bounds;
		//			rectSecond.X = _widthSplit;
		//
		//			using ( Brush br = new SolidBrush(e.BackColor) )
		//				e.Graphics.FillRectangle( br, rectFirst);
		//
		//			if(!this.DesignMode)
		//			{
		//				using ( Brush brFore = new SolidBrush(e.ForeColor) )
		//				using ( Brush brBlack = new SolidBrush(Color.Black) )
		//				using ( Pen pen = new Pen( Color.DarkGray))
		//				{
		//					//First column
		//					StringFormat sf = new StringFormat();
		//					sf.LineAlignment = StringAlignment.Center;
		//					sf.FormatFlags = StringFormatFlags.LineLimit;
		//					string text = "";
		//					TextShortcut ts = (TextShortcut)this.Items[e.Index];
		//					if ( ts.ShortName != "")
		//						text = ts.ShortName;
		//					else
		//						text = (string)ts.MethodName;
		//				
		//					e.Graphics.DrawString( text, e.Font, brFore, rectFirst, sf);
		//
		//					//Second column
		//					Brush br = null;
		//					if (state != DrawItemState.Selected)
		//						br = brBlack;
		//					else
		//						br = brFore;
		//
		//					int key = GetItemShortcut( e.Index);
		//					e.Graphics.DrawString( KeyToString(key), e.Font, br, rectSecond, sf);
		//
		//					e.Graphics.DrawLine( pen, _widthSplit, e.Bounds.Y, _widthSplit, e.Bounds.Y + e.Bounds.Height - 1);
		//					e.Graphics.DrawLine( pen, e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1, e.Bounds.Width, e.Bounds.Y + e.Bounds.Height - 1);
		//				}
		//			}
		//
		//		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var off = Math.Abs(e.X - _widthSplit);

			Cursor = off <= _offsetSplit || _changeSize
				? Cursors.VSplit
				: Cursors.Default;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			var off = Math.Abs(e.X - _widthSplit);

			if (off <= _offsetSplit)
			{
				_changeSize = true;
				Capture = true;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (_widthSplit != e.X && _changeSize)
			{
				_widthSplit = e.X;
				_changeSize = false;

				Invalidate();
			}

			Capture = false;
		}
		#endregion
	}
}