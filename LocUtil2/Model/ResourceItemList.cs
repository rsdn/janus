using System.Collections;

namespace Rsdn.LocUtil.Model
{
	/// <summary>
	/// Список ресурсов.
	/// </summary>
	public class ResourceItemList : CollectionBase
	{
		private readonly static ItemNameComparer _comparer = new ItemNameComparer();

		/// <summary>
		/// Доступ к элементу по индексу.
		/// </summary>
		public ResourceItem this[int index]
		{
			get { return (ResourceItem)List[index]; }
		}

		/// <summary>
		/// Добавить элемент.
		/// </summary>
		public void Add(ResourceItem item)
		{
			int idx = InnerList.BinarySearch(item, _comparer);
			if (idx >= 0)
				return;
			List.Insert(-idx - 1, item);
		}

		/// <summary>
		/// Получить индекс элемента.
		/// </summary>
		public int IndexOf(ResourceItem item)
		{
			return List.IndexOf(item);
		}
	}
}
