using System.Collections;

namespace Rsdn.LocUtil.Model
{
	/// <summary>
	/// Компаратор элементов модели по имени.
	/// </summary>
	public class ItemNameComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			return ((ItemBase)x).Name.CompareTo(((ItemBase)y).Name);
		}
	}
}