using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{
	public partial class ComboListBox : Control
	{
		private readonly ObservableList<object> _items;

		public ComboListBox()
		{
			InitializeComponent();
			_items = new ObservableList<object>();
			_items.Changed += (sender, e) => ChangedList();
		}

		#region Отслеживаем изменения Item'ов
		internal void ChangedList()
		{
			var arr = _items.ToArray();

			try
			{
				lbxLog.BeginUpdate();
				lbxLog.Items.Clear();
				lbxLog.Items.AddRange(arr);

				cbxLog.BeginUpdate();
				cbxLog.Items.Clear();
				cbxLog.Items.AddRange(arr);
			}
			finally
			{
				lbxLog.EndUpdate();
				cbxLog.EndUpdate();
			}
		}
		#endregion

		#region Делаем плацдарм для рисования Item'ов
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{ }

		private void Log_DrawItem(object sender, DrawItemEventArgs e)
		{
			OnDrawItem(e);
		}
		#endregion

		#region Делаем вид, что есть событие SelectedItemChanged
		public event EventHandler SelectedIndexChanged;

		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			if (SelectedIndexChanged != null)
				SelectedIndexChanged(this, e);
		}

		private void lbxLog_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnSelectedIndexChanged(e);
		}

		private void Log_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbxLog.Visible)
			{
				if (lbxLog.Items.Count != 0)
					lbxLog.SelectedIndex = 0;
				if (cbxLog.Items.Count != 0)
					cbxLog.SelectedIndex = 0;
			}
		}
		#endregion

		#region Делаем новое событие MouseClick
		public new event MouseEventHandler MouseClick;

		protected new virtual void OnMouseClick(MouseEventArgs e)
		{
			if (MouseClick != null)
				MouseClick(this, e);
		}

		private void lbxLog_MouseClick(object sender, MouseEventArgs e)
		{
			OnMouseClick(e);
		}
		#endregion

		#region Делаем новое событие MouseUp
		public new event MouseEventHandler MouseUp;

		protected new virtual void OnMouseUp(MouseEventArgs e)
		{
			if (MouseUp != null)
				MouseUp(this, e);
		}

		private void lbxLog_MouseUp(object sender, MouseEventArgs e)
		{
			OnMouseUp(e);
		}
		#endregion

		#region Делаем вид, что есть метод IndexFromPoint
		public int IndexFromPoint(Point pt)
		{
			return lbxLog.Visible ? lbxLog.IndexFromPoint(pt) : -1;
		}

		public int IndexFromPoint(int x, int y)
		{
			return lbxLog.Visible ? lbxLog.IndexFromPoint(x, y) : -1;
		}
		#endregion

		public int ItemHeight
		{
			get { return cbxLog.ItemHeight; }
			set
			{
				lbxLog.ItemHeight = value;
				cbxLog.ItemHeight = value;
			}
		}

		public int SelectedIndex
		{
			get { return lbxLog.SelectedIndex; }
			set
			{
				lbxLog.SelectedIndex = value;
				cbxLog.SelectedIndex = value;
			}
		}

		public int TopIndex
		{
			get { return lbxLog.TopIndex; }
			set { lbxLog.TopIndex = value; }
		}

		public IList<object> Items
		{
			get { return _items; }
		}

		#region Делаем подмену компонентов, в зависимости от размеров
		protected override void OnResize(EventArgs e)
		{
			cbxLog.Visible = ClientRectangle.Height <= cbxLog.Height;
			lbxLog.Visible = !cbxLog.Visible;

			lbxLog.Height = ClientRectangle.Height;
			lbxLog.Width = ClientRectangle.Width;
			cbxLog.Width = ClientRectangle.Width;

			if (cbxLog.Visible)
				Log_SelectedIndexChanged(this, e);

			base.OnResize(e);
		}
		#endregion
	}
}