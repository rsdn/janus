using System.Runtime.InteropServices;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Добавление элементов в системное меню
	/// </summary>
	public static class SystemMenuHelper
	{
		private const int MF_SEPARATOR = 0x800;
		private const int MF_STRING = 0x0;

		[DllImport("user32.dll")]
		private static extern int GetSystemMenu(int hwnd, int bRevert);

		[DllImport("user32.dll")]
		private static extern int AppendMenu(int hMenu, int Flagsw, int IDNewItem, string lpNewItem);

		public static void AddItemToMenu(int handle, int itemId, string itemCaption, bool needSeparator)
		{
			var sysMenu = GetSystemMenu(handle, 0);
			if (needSeparator)
				AppendMenu(sysMenu, MF_SEPARATOR, 0, null);

			AppendMenu(sysMenu, MF_STRING, itemId, itemCaption);
		}
	}
}