using System;
using System.Runtime.InteropServices;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Активирует окно.
	/// </summary>
	public static class WindowActivator
	{
		#region CmdShow enum
		public enum CmdShow
		{
			SW_SHOWNOACTIVATE = 4,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
		}
		#endregion

		[DllImport("User32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("User32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("User32.dll")]
		private static extern bool IsIconic(IntPtr hWnd);

		[DllImport("User32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, CmdShow cmdShow);

		public static void ActivateWindow(string windowTitle)
		{
			var hWnd = FindWindow(null, windowTitle);

			if (IsIconic(hWnd))
				ShowWindow(hWnd, CmdShow.SW_RESTORE);

			SetForegroundWindow(hWnd);
		}
	}
}