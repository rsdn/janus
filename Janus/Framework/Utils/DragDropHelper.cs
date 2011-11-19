using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{
	public static class DragDropHelper
	{
		#region GetDropEffect
		public static DragDropEffects GetDropEffect(Keys keys, bool defaultMove)
		{
			if ((keys & Keys.Shift) == Keys.Shift)
				return DragDropEffects.Move;
			if ((keys & Keys.Control) == Keys.Control)
				return DragDropEffects.Copy;
			if ((keys & Keys.Alt) == Keys.Alt)
				return DragDropEffects.Link;

			return defaultMove ? DragDropEffects.Move : DragDropEffects.Copy;
		}

		public static DragDropEffects GetDropEffect(int keyState, bool defaultMove)
		{
			return GetDropEffect(GetModifierKeys(keyState), defaultMove);
		}

		public static DragDropEffects GetDropEffect(bool defaultMove)
		{
			return GetDropEffect(Control.ModifierKeys, defaultMove);
		}

		public static DragDropEffects GetDropEffect()
		{
			return GetDropEffect(Control.ModifierKeys, false);
		}
		#endregion GetDropEffect

		#region GetKeyState
		public static int GetKeyState(MouseButtons buttons, Keys keys)
		{
			// See DragEventArgs.KeyState property
			var keyState = 0;
			if ((buttons & MouseButtons.Left) == MouseButtons.Left)
				keyState |= 1;
			if ((buttons & MouseButtons.Right) == MouseButtons.Right)
				keyState |= 2;
			if ((keys & Keys.ShiftKey) == Keys.ShiftKey)
				keyState |= 4;
			if ((keys & Keys.ControlKey) == Keys.ControlKey)
				keyState |= 8;
			if ((buttons & MouseButtons.Middle) == MouseButtons.Middle)
				keyState |= 16;
			if ((keys & Keys.Alt) == Keys.Alt)
				keyState |= 32;
			return keyState;
		}

		public static int GetKeyState()
		{
			return GetKeyState(Control.MouseButtons, Control.ModifierKeys);
		}
		#endregion GetKeyState

		#region Keys Informaton
		public static Keys GetModifierKeys(int keyState)
		{
			var keys = Keys.None;
			if ((keyState & 4) == 4)
				keys |= Keys.Shift;
			if ((keyState & 8) == 8)
				keys |= Keys.Control;
			if ((keyState & 32) == 32)
				keys |= Keys.Alt;
			return keys;
		}

		public static MouseButtons GetMouseButtons(int keyState)
		{
			var buttons = MouseButtons.None;
			if ((keyState & 1) == 1)
				buttons |= MouseButtons.Left;
			if ((keyState & 2) == 2)
				buttons |= MouseButtons.Right;
			if ((keyState & 16) == 16)
				buttons |= MouseButtons.Middle;
			return buttons;
		}
		#endregion Keys Informaton
	}
}