using System;
using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for ITicker.
	/// </summary>
	public interface ITicker
	{
		Graphics CreateGraphics();
		void RedrawIndicator(ITickerIndicator ti);
	}
}
