using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Rsdn.Janus.Framework;
using Rsdn.Scintilla;

namespace Rsdn.Janus
{
	/// <summary>
	/// Add some janus-specific functionality.
	/// </summary>
	public class JanusScintilla : ScintillaEditor
	{
		private const int WM_KEYDOWN = 0x0100;
		private const int WM_UNDO = 0x0304;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Process janus-specific format of clipboard.
		/// </summary>
		public override bool PreProcessMessage(ref Message msg)
		{
			if ((msg.Msg == WM_KEYDOWN))
			{
				var dnk = ((Keys)msg.WParam.ToInt32()) | ModifierKeys;
				if ((dnk == (Keys.Control | Keys.V)) || (dnk == (Keys.Shift | Keys.Insert)))
				{
					var dto = Clipboard.GetDataObject();
					if (dto.GetDataPresent(ClipboardHelper.MessageSubjectFormat))
					{
						var url = (string)dto.GetData(DataFormats.Text);
						if (url.StartsWith("www."))
							url = "http://" + url;
						var subj = (string)dto.GetData(ClipboardHelper.MessageSubjectFormat);
						Selection.Text = string.Format("[url={0}]{1}[/url]", url, subj);
						return true;
					}
				}
			}
			return base.PreProcessMessage(ref msg);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Alt && (e.KeyCode == Keys.Back))
			{
				e.SuppressKeyPress = true;
				SendMessage(Handle, WM_UNDO, new IntPtr(), new IntPtr());
			}
			base.OnKeyDown(e);
		}
	}
}
