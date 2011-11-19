using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обертка, защищающая <see cref="IDictionary{TKey,TValue}"/> от изменений.
	/// </summary>
	public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
	{
		private readonly IDictionary<TKey, TValue> _dictionary;

		public ReadOnlyDictionary([NotNull] IDictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");

			_dictionary = dictionary;
		}

		#region IEnumerable Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		} 

		#endregion

		#region ICollection<T> Members

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return _dictionary.Contains(item);
		}

		public int Count
		{
			get { return _dictionary.Count; }
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			throw new NotSupportedException();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			_dictionary.CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get { return true; }
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region IDictionary<K,V> Members

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue result)
		{
			return _dictionary.TryGetValue(key, out result);
		}

		public TValue this[TKey key]
		{
			get { return _dictionary[key]; }
			set { throw new NotSupportedException(); }
		}

		public ICollection<TKey> Keys
		{
			get { return _dictionary.Keys; }
		}

		public ICollection<TValue> Values
		{
			get { return _dictionary.Values; }
		}

		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			throw new NotSupportedException();
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get { return _dictionary.Keys; }
		}

		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			throw new NotSupportedException();
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get { return _dictionary.Values; }
		}

		#endregion
	}
}