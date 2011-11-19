using System;
using System.Collections.Generic;
using System.Text;

namespace Peak.TreeGrid
{
	public interface IIndexable<K, T>
	{
		T this[K index] { get;}
	}
}
