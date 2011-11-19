using System;
using System.Collections;
using System.Collections.Generic;

namespace Rsdn.Janus.Framework
{
	public class CollectionBase<T> :
		IList<T>, IList
	{
		private readonly List<T> _innerList;
		private bool _validateOnRemove;

		public CollectionBase()
			: this(10)
		{}

		public CollectionBase(int initialCapacity)
		{
			_innerList = new List<T>(initialCapacity);
			_validateOnRemove = false;
		}

		protected List<T> InnerList
		{
			get { return _innerList; }
		}

		protected bool ValidateOnRemove
		{
			get { return _validateOnRemove; }
			set { _validateOnRemove = value; }
		}

		#region IList Members
		public virtual int Add(object value)
		{
			var index = _innerList.Count;

			if (!OnValidate((T)value))
				return -1;
			if (!OnInsert(index, (T)value))
				return -1;

			index = ((IList)_innerList).Add(value);
			OnInsertComplete(index, (T)value);
			return index;
		}

		public virtual bool Contains(object value)
		{
			return ((IList)_innerList).Contains(value);
		}

		public virtual int IndexOf(object value)
		{
			return ((IList)_innerList).IndexOf(value);
		}

		public virtual void Insert(int index, object value)
		{
			if (!OnValidate((T)value))
				return;
			if (!OnInsert(index, (T)value))
				return;

			((IList)_innerList).Insert(index, value);
			OnInsertComplete(index, (T)value);
		}

		public virtual bool IsFixedSize
		{
			get { return ((IList)_innerList).IsFixedSize; }
		}


		public virtual void Remove(object value)
		{
			var index = _innerList.IndexOf((T)value);

			if (index < 0)
				return;

			if (_validateOnRemove && !OnValidate((T)value))
				return;
			if (!OnRemove(index, (T)value))
				return;

			((IList)_innerList).Remove(value);
			OnRemoveComplete(index, (T)value);
		}

		object IList.this[int index]
		{
			get { return _innerList[index]; }

			set
			{
				var oldValue = _innerList[index];
				if (ReferenceEquals(oldValue, value))
					return;

				if (!OnValidate((T)value))
					return;
				if (!OnSet(index, oldValue, (T)value))
					return;

				_innerList[index] = (T)value;
				OnSetComplete(index, oldValue, (T)value);
			}
		}

		public virtual void CopyTo(Array array, int index)
		{
			((ICollection)_innerList).CopyTo(array, index);
		}

		public virtual bool IsSynchronized
		{
			get { return ((ICollection)_innerList).IsSynchronized; }
		}

		public virtual object SyncRoot
		{
			get { return ((ICollection)_innerList).SyncRoot; }
		}
		#endregion

		#region IList<T> Members
		public virtual int IndexOf(T item)
		{
			return _innerList.IndexOf(item);
		}

		public virtual void Insert(int index, T item)
		{
			if (!OnValidate(item))
				return;
			if (!OnInsert(index, item))
				return;

			_innerList.Insert(index, item);
			OnInsertComplete(index, item);
		}

		public virtual void RemoveAt(int index)
		{
			var value = _innerList[index];

			if (_validateOnRemove && !OnValidate(value))
				return;
			if (!OnRemove(index, value))
				return;

			_innerList.RemoveAt(index);
			OnRemoveComplete(index, value);
		}

		public virtual T this[int index]
		{
			get { return _innerList[index]; }
			set
			{
				var oldValue = _innerList[index];
				if (ReferenceEquals(oldValue, value))
					return;

				if (!OnValidate(value))
					return;
				if (!OnSet(index, oldValue, value))
					return;

				_innerList[index] = value;
				OnSetComplete(index, oldValue, value);
			}
		}

		public virtual void Clear()
		{
			if (!OnClear())
				return;

			_innerList.Clear();

			OnClearComplete();
		}

		public virtual void Add(T item)
		{
			if (!OnValidate(item))
				return;
			if (!OnInsert(_innerList.Count, item))
				return;

			_innerList.Add(item);
			OnInsertComplete(_innerList.Count - 1, item);
		}

		public virtual bool Contains(T item)
		{
			return _innerList.Contains(item);
		}

		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			_innerList.CopyTo(array, arrayIndex);
		}

		public virtual int Count
		{
			get { return _innerList.Count; }
		}

		public virtual bool IsReadOnly
		{
			get { return ((ICollection<T>)_innerList).IsReadOnly; }
		}

		public virtual bool Remove(T item)
		{
			var index = _innerList.IndexOf(item);

			if (index < 0)
				return false;

			if (_validateOnRemove && !OnValidate(item))
				return false;
			if (!OnRemove(index, item))
				return false;

			_innerList.Remove(item);
			OnRemoveComplete(index, item);
			return true;
		}

		public virtual IEnumerator<T> GetEnumerator()
		{
			return _innerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_innerList).GetEnumerator();
		}
		#endregion

		public T[] ToArray()
		{
			return _innerList.ToArray();
		}

		#region Sort
		public void Sort(Comparison<T> comparison)
		{
			_innerList.Sort(comparison);
		}

		public void Sort(IComparer<T> comparer)
		{
			_innerList.Sort(comparer);
		}

		public void Sort(int index, int count, IComparer<T> comparer)
		{
			_innerList.Sort(index, count, comparer);
		}
		#endregion

		#region Notification Events
		protected virtual bool OnClear()
		{
			return true;
		}

		protected virtual void OnClearComplete()
		{}

		protected virtual bool OnInsert(int index, T value)
		{
			return true;
		}

		protected virtual void OnInsertComplete(int index, T value)
		{}

		protected virtual bool OnRemove(int index, T value)
		{
			return true;
		}

		protected virtual void OnRemoveComplete(int index, T value)
		{}

		protected virtual bool OnSet(int index, T oldValue, T value)
		{
			return true;
		}

		protected virtual void OnSetComplete(int index, T oldValue, T value)
		{}

		protected virtual bool OnValidate(T value)
		{
			return true;
		}
		#endregion
	}
}