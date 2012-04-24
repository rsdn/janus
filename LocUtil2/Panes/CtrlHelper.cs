using System;
using System.Drawing;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Summary description for CtrlHelper.
	/// 
	/// Ported by Andre from VB and adapted
	/// </summary>
	public static class CtrlHelper
	{
		/// <summary>
		/// 
		/// </summary>
		public static RectangleF CheckedRectangleF(float x, float y, float width, float height)
		{
			return new RectangleF(x, y,
				Convert.ToSingle(width <= 0 ? 1 : width),
				Convert.ToSingle(height <= 0 ? 1 : height));
		}

		/// <summary>
		/// 
		/// </summary>
		public static Rectangle CheckedRectangle(int x, int y, int width, int height)
		{
			return new Rectangle(x, y,
				(width <= 0 ? 1 : width),
				(height <= 0 ? 1 : height));
		}
	}
}