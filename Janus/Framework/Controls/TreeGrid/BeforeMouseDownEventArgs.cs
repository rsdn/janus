using System.Diagnostics;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	public class BeforeMouseDownEventArgs : MouseEventArgs
	{
		public BeforeMouseDownEventArgs(
			bool cancel, MouseButtons button, int clicks, int x, int y, int delta)
			: base(button, clicks, x, y, delta)
		{
			_cancel = cancel;
		}

		#region Cancel property
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _cancel;

		public bool Cancel
		{
			[DebuggerHidden]
			get { return _cancel; }
			[DebuggerHidden]
			set { _cancel = value; }
		}
		#endregion
	}
}