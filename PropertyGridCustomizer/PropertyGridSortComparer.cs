using System;
using System.Collections;
using System.ComponentModel;

namespace Rsdn.Janus
{
	internal class PropertyGridSortComparer :
		IComparer
	{
		#region IComparer Members
		public int Compare(object x, object y)
		{
			var attX = ((PropertyDescriptor)x).Attributes;
			var attY = ((PropertyDescriptor)y).Attributes;

			int indexX = 0, indexY = 0;

			foreach (Attribute at in attX)
				if (at is SortIndexAttribute)
				{
					indexX = (at as SortIndexAttribute).SortIndex;
					break;
				}

			foreach (Attribute at in attY)
				if (at is SortIndexAttribute)
				{
					indexY = (at as SortIndexAttribute).SortIndex;
					break;
				}
			return indexX - indexY;
		}
		#endregion
	}
}