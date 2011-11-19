using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Peak.TreeGrid
{
	public class ColumnCollection: Collection<Column>
	{
		internal TreeGrid1 owner = null;

		public ColumnCollection(TreeGrid1 owner)
		{
			this.owner = owner;
		}

		protected override void ClearItems()
		{
			base.ClearItems();
			owner.OnColumnsChanged(EventArgs.Empty);
		}

		protected override void InsertItem(int index, Column item)
		{
			base.InsertItem(index, item);
			owner.OnColumnsChanged(EventArgs.Empty);
			if (item != null)
				item.columns = this;
		}

		protected override void RemoveItem(int index)
		{
			if (this[index] != null)
				this[index].columns = null;
			base.RemoveItem(index);
			owner.OnColumnsChanged(EventArgs.Empty);
		}

		protected override void SetItem(int index, Column item)
		{
			if (this[index] != null)
				this[index].columns = null;
			base.SetItem(index, item);
			if (this[index] != null)
				this[index].columns = this;
			owner.OnColumnsChanged(EventArgs.Empty);
		}

		public List<T> ConvertAll<T>(Converter<Column, T> converter)
		{
			return new List<Column>(this).ConvertAll(converter);
		}
	}
}
