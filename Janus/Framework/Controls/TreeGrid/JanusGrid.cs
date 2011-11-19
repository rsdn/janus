using System;
using System.ComponentModel;
using System.Windows.Forms;

using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Версия TreeGrid для януса.
	/// </summary>
	public class JanusGrid : TreeGrid.TreeGrid
	{
		#region ColumnsOrder property
		/// <summary>
		/// Порядок следования колонок.
		/// </summary>
		[Browsable(false)]
		public int[] ColumnsOrder
		{
			get
			{
				var res = new int[Columns.Count];

				for (var i = 0; i < Columns.Count; i++)
					res[i] = Columns[i].DisplayIndex;

				return res;
			}
			set
			{
				for (var i = 0; i < value.Length && i < Columns.Count; i++)
					Columns[i].DisplayIndex = value[i];
			}
		}
		#endregion

		#region ColumnsWidth property
		/// <summary>
		/// Ширины колонок.
		/// </summary>
		[Browsable(false)]
		public int[] ColumnsWidth
		{
			get
			{
				var res = new int[Columns.Count];
				for (var i = 0; i < res.Length; i++)
					res[i] = Columns[i].Width;
				return res;
			}
			set
			{
				for (var i = 0; i < value.Length && i < Columns.Count; i++)
					Columns[i].Width = value[i];
			}
		}
		#endregion

		#region Сообщения Windows
		private const int WM_LBUTTONDBLCLK = 0x0203;
		private const int WM_LBUTTONDOWN = 0x0201;
		//private const int WM_LBUTTONUP = 0x0202;
		private const int WM_MBUTTONDBLCLK = 0x0209;
		private const int WM_MBUTTONDOWN = 0x0207;
		//private const int WM_MBUTTONUP = 0x0208;
		//private const int WM_MOUSEMOVE = 0x0200;
		private const int WM_RBUTTONDBLCLK = 0x0206;
		private const int WM_RBUTTONDOWN = 0x0204;
		//private const int WM_RBUTTONUP = 0x0205;
		#endregion

		#region Событие BeforeMouseDownEvent
		public event EventHandler<BeforeMouseDownEventArgs> BeforeMouseDown;

		/// <summary>
		/// Вызывается перед обработкой сообщения нажатия мыши и позволяет
		/// отменить его.
		/// </summary>
		protected virtual bool OnBeforeMouseDownEvent(
			MouseButtons button, int clicks, int x, int y)
		{
			if (BeforeMouseDown != null)
			{
				var e = new BeforeMouseDownEventArgs(
					false, button, clicks, x, y, 0);

				BeforeMouseDown(this, e);

				return e.Cancel;
			}

			return false;
		}
		#endregion

		#region WndProc
		/// <summary>
		/// Обработчик сообщений виндовс.
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			var clicks = 1;

			switch (m.Msg)
			{
				case WM_MBUTTONDBLCLK:
				case WM_LBUTTONDBLCLK:
				case WM_RBUTTONDBLCLK:
					clicks = 2;
					goto case WM_LBUTTONDOWN;
				case WM_LBUTTONDOWN:
				case WM_RBUTTONDOWN:
				case WM_MBUTTONDOWN:
					if (OnBeforeMouseDownEvent(
						MouseButtons, clicks,
						BitUtils.LoWordAsInt(m.LParam),
						BitUtils.HiWordAsInt(m.LParam)))
						return;

					break;
			}

			base.WndProc(ref m);
		}
		#endregion

		#region Overridden methods
		protected override NodeDrawData RetrieveData(ITreeNode node)
		{
			var config = Config.Instance.ForumDisplayConfig;
			var nodeDrawData = base.RetrieveData(node);


			if (!config.HighlightParentMessage && !config.HighlightChildMessages)
				return nodeDrawData;

			if (node is IMsg)
			{
				var style = Config.Instance.StyleConfig;

				if (ActiveNode != null && ActiveNode.Parent == node)
				{
					if (config.HighlightParentMessage)
						nodeDrawData.NodeInfo.BackColor = style.ParentActiveMessageBackColor;
				}
				else if (node.Parent != null && node.Parent == ActiveNode)
					if (config.HighlightChildMessages)
						nodeDrawData.NodeInfo.BackColor = style.ChildActiveMessageBackColor;
			}

			return nodeDrawData;
		}

		protected override void OnAfterActivateNode(ITreeNode activatedNode)
		{
			base.OnAfterActivateNode(activatedNode);

			if (activatedNode is IMsg)
			{
				var config = Config.Instance.ForumDisplayConfig;

				// Если не обновлять, то при смене активной ветки не перерисовываются
				// ранее подкрашенные ветки
				if (config.HighlightParentMessage || config.HighlightChildMessages)
					Refresh();
			}
		}
		#endregion
	}
}