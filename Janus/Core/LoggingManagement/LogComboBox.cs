using System.Drawing;
using System.Windows.Forms;

using Rsdn.Janus.Framework;

namespace Rsdn.Janus
{
	/// <summary>
	/// ComboBox для истории
	/// </summary>
	public class LogComboBox : ComboListBox
	{
		private Size _iconSize = new Size(16, 16);

		public LogComboBox()
		{
			ItemHeight = _iconSize.Height + 2;
		}
		
		public void AddItem(object sender, LogEventArgs e)
		{
			MethodInvoker mi =
				() =>
				{
					if (e.Item.Type == LogEventType.Track)
						return;
					Items.Insert(0, e.Item);
					SelectedIndex = 0;
					TopIndex = 0;
				};

			if (InvokeRequired)
				Invoke(mi);
			else
				mi();
		}

		#region Protected methods

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			var tc = 
				(e.State & DrawItemState.Selected) == 0
					? Color.Black
					: SystemColors.HighlightText;

			var b = e.Bounds;
			e.DrawBackground();

			if (e.Index < 0)
				return;
			var item = (LogItem)Items[e.Index];

			e.Graphics.DrawIcon(LogItemIcon(item),
				new Rectangle(b.X + 1, b.Y + 1, _iconSize.Width, _iconSize.Height));

			e.Graphics.DrawString(item.Message, Font, new SolidBrush(tc),
				b.X + _iconSize.Width + 3, b.Y + 1);
		}

		#endregion

		#region Private methods

		private static Icon LogItemIcon(LogItem item)
		{
			switch (item.Type)
			{
				case LogEventType.Warning:  return SystemIcons.Warning;
				case LogEventType.Error:	return SystemIcons.Error;
				default:					return SystemIcons.Information;
			}
		}
		#endregion
	}
}
