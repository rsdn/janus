using System;
using System.Collections.Generic;
using System.Text;

namespace Peak.TreeGrid
{
	public interface IDualIndexable<K1, K2, T>
	{
		T this[K1 index1, K2 index2] { get; }
	}
}
