using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Меняет реакцию контролов на колесо, рассылая сообщение 
	/// не активному контролу, а контролу под мышкой.
	/// </summary>
	public class WheelDispatcher : IMessageFilter
	{
		#region IMessageFilter Members
		/// <summary>
		/// Предварительная фильтрация сообщения
		/// </summary>
		/// <param name="m">сообщение</param>
		/// <returns>true - прервать обработку, false - продолжать обработку</returns>
		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == NativeMethods.WM_MOUSEWHEEL)
			{
				var point = new NativeMethods.POINT
				{
					x = ((short)((uint)m.LParam & 0xFFFF)),
					y = ((short)(((uint)m.LParam & 0xFFFF0000) >> 16))
				};
				m.HWnd = NativeMethods.WindowFromPoint(point);
				NativeMethods.SendMessage(m.HWnd, m.Msg, m.WParam, m.LParam);
				return true;
			}
			return false;
		}
		#endregion
	}
}