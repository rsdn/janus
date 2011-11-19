using System;
using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for ITickerIndicator.
	/// </summary>
	public interface ITickerIndicator
	{
		ITicker Owner {get; set;}
		Rectangle Bounds {get; set;}
		Point Location {get; set;}
		Size Size {get; set;}
		Color ForeColor {get; set;}
		Color BackColor {get; set;}
		void Draw(Graphics g);
	}
}
