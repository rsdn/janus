using System;
using System.Collections.Generic;
using System.Text;

namespace Peak.TreeGrid
{
	public class DelegateIndexable<K, T>: IIndexable<K, T>
	{
		public delegate T GetValue(K index);
		public delegate void SetValue(K index, T value);

		private GetValue getValue;
		private SetValue setValue;

		public T this[K index]
		{
			get { return getValue(index); }
			set { setValue(index, value); }
		}

		public DelegateIndexable(GetValue getValue, SetValue setValue)
		{
			this.getValue = getValue;
			this.setValue = setValue;
		}
	}

}
