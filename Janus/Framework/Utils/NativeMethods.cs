using System;
using System.Runtime.InteropServices;

namespace Rsdn.Janus.Framework
{
	internal static class NativeMethods
	{
		public const int SC_HOTKEY = 0xF150;
		public const int WM_CLOSE = 0x0010;
		public const int WM_MOUSEWHEEL = 0x020A;
		public const int WM_SYSCOMMAND = 0x0112;
		public const int WM_USER = 0x0400;

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(POINT point);

		#region Nested type: POINT
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}
		#endregion
	}
}