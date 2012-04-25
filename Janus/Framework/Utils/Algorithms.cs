using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rsdn.Janus.Framework
{
	internal static class Algorithms
	{
		#region GetFirstElements<T>(collection, count)
		public static ICollection<T> GetFirstElements<T>(ICollection<T> collection, int count)
		{
			return new FirstElementsCollection<T>(collection, count);
		}

		private class FirstElementsCollection<T> : ICollection<T>
		{
			private readonly ICollection<T> _collection;
			private readonly int _count;

			public FirstElementsCollection(ICollection<T> collection, int count)
			{
				_count = count;
				_collection = collection;
			}

			#region ICollection<T> Members
			public int Count
			{
				get { return _count; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}

			public bool Contains(T item)
			{
				throw new InvalidOperationException();
			}

			public void CopyTo(T[] array, int arrayIndex)
			{
				throw new InvalidOperationException();
			}

			public void Add(T item)
			{
				throw new InvalidOperationException();
			}

			public void Clear()
			{
				throw new InvalidOperationException();
			}

			public bool Remove(T item)
			{
				throw new InvalidOperationException();
			}

			public IEnumerator<T> GetEnumerator()
			{
				return _collection.Take(_count).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
			#endregion
		}
		#endregion
	}
}