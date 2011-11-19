using System.Collections;

namespace Rsdn.LocUtil.Model
{
	/// <summary>
	/// Список категорий.
	/// </summary>
	public class CategoryList : CollectionBase
	{
		private static readonly ItemNameComparer _comparer = new ItemNameComparer();

		/// <summary>
		/// Доступ к элементам по индексу
		/// </summary>
		public Category this[int index]
		{
			get { return (Category)List[index]; }
		}

		/// <summary>
		/// Добавить элемент.
		/// </summary>
		public void Add(Category category)
		{
			int idx = InnerList.BinarySearch(category, _comparer);
			if (idx >= 0)
				return;
			List.Insert(-idx - 1, category);
		}

		/// <summary>
		/// Получить индекс элемента.
		/// </summary>
		public int IndexOf(Category cat)
		{
			return List.IndexOf(cat);
		}
	}
}
