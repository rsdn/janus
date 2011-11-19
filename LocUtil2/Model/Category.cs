using System;
using System.Collections;
using System.Globalization;
using Rsdn.TreeGrid;

namespace Rsdn.LocUtil.Model
{
	/// <summary>
	/// Категория ресурсов.
	/// </summary>
	public class Category : ItemBase, ITreeNode
	{
		private readonly CategoryList _categories = new CategoryList();
		private readonly ResourceItemList _resourceItems = new ResourceItemList();
		private NodeFlags _flags;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public Category(string name, Category parent)
			: base(name, parent)
		{
		}

		/// <summary>
		/// Подчиненные категории.
		/// </summary>
		public CategoryList Categories
		{
			get { return _categories; }
		}

		/// <summary>
		/// Подчиненные ресурсы.
		/// </summary>
		public ResourceItemList ResourceItems
		{
			get { return _resourceItems; }
		}

		/// <summary>
		/// Получить список всех культур.
		/// </summary>
		public CultureInfo[] GetCultures()
		{
			Hashtable ht = new Hashtable();
			foreach (Category c in _categories)
				foreach (CultureInfo ci in c.GetCultures())
					ht[ci] = null;
			foreach (ResourceItem ri in _resourceItems)
				foreach (CultureInfo ci in ri.ValueCollection.GetCultures())
					ht[ci] = null;
			CultureInfo[] res = new CultureInfo[ht.Count];
			ht.Keys.CopyTo(res, 0);
			return res;
		}

		/// <summary>
		/// Получить все вложенные элементы.
		/// </summary>
		public ResourceItem[] GetAllItems()
		{
			ArrayList al = new ArrayList(_resourceItems);
			foreach (Category c in _categories)
				al.AddRange(c.GetAllItems());
			return (ResourceItem[])al.ToArray(typeof (ResourceItem));
		}

		/// <summary>
		/// Получить количество всех содержащихся ресурсов.
		/// </summary>
		/// <returns></returns>
		public int GetAllResCount()
		{
			int cnt = 0;
			foreach (Category cat in Categories)
				cnt += cat.GetAllResCount();
			cnt += ResourceItems.Count;
			return cnt;
		}

		internal void DeleteItem(ResourceItem item)
		{
			int idx = _resourceItems.IndexOf(item);
			if (idx == -1)
				return;
			_resourceItems.RemoveAt(idx);
			DeleteIfEmpty();
		}

		internal void DeleteCategory(Category category)
		{
			int idx = _categories.IndexOf(category);
			if (idx == -1)
				return;
			_categories.RemoveAt(idx);
			DeleteIfEmpty();
		}

		private void DeleteIfEmpty()
		{
			if (Parent != null && _resourceItems.Count == 0 && _categories.Count == 0)
				Parent.DeleteCategory(this);
		}

		#region ICollection
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)_categories).CopyTo(array, index);
		}

		int ICollection.Count
		{
			get { return _categories.Count; }
		}

		object ICollection.SyncRoot
		{
			get { return ((ICollection)_categories).SyncRoot; }
		}

		bool ICollection.IsSynchronized
		{
			get { return ((ICollection)_categories).IsSynchronized; }
		}
		#endregion

		#region ITreeNode
		ITreeNode ITreeNode.Parent
		{
			get { return Parent; }
		}

		NodeFlags ITreeNode.Flags
		{
			get { return _flags; }
			set { _flags = value; }
		}

		bool ITreeNode.HasChildren
		{
			get { return _categories.Count > 0; }
		}

		ITreeNode ITreeNode.this[int index]
		{
			get { return _categories[index]; }
		}
		#endregion

		#region IEnumerable
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _categories.GetEnumerator();
		}
		#endregion
	}
}