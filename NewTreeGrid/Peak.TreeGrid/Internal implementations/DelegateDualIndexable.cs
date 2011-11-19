using System;
using System.Collections.Generic;
using System.Text;

namespace Peak.TreeGrid
{
	public class DelegateDualIndexable<K1, K2, T> : IDualIndexable<K1, K2, T>
	{
		public delegate T GetValue(K1 index1, K2 index2);
		public delegate void SetValue(K1 index1, K2 index2, T value);

		GetValue getValue;
		SetValue setValue;

		public T this[K1 index1, K2 index2]
		{
			get { return getValue(index1, index2); }
			set { setValue(index1, index2, value); }
		}

		public DelegateDualIndexable(GetValue getValue, SetValue setValue)
		{
			this.getValue = getValue;
			this.setValue = setValue;
		}
	}
}
