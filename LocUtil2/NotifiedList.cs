using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace Rsdn.LocUtil
{
	internal class NotifiedList<T> : Collection<T>
	{
		public NotifiedList(IList<T> list) : base(new List<T>(list))
		{
		}

		private List<T> InnerList
		{
			get { return (List<T>)Items; }
		}

		public event EventHandler SizeChanged;

		public void Sort(Comparison<T> comparison)
		{
			InnerList.Sort(comparison);
		}

		public T[] ToArray()
		{
			return InnerList.ToArray();
		}

		protected virtual void OnSizeChanged()
		{
			if (SizeChanged != null)
				SizeChanged(this, EventArgs.Empty);
		}

		protected override void ClearItems()
		{
			base.ClearItems();
			OnSizeChanged();
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			OnSizeChanged();
		}

		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
			OnSizeChanged();
		}
	}
}
