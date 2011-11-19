using System;
using System.Collections.Generic;
using System.Linq;

namespace Rsdn.Janus
{
	/// <summary>
	/// Список, имеющий событие, оповещающее о произведенных в нем изменениях.
	/// </summary>
	public class ObservableList<T> : IList<T>
	{
		private readonly List<T> _list;

		public event ListChangedEventHandler<T> Changed;

		#region Конструкторы

		public ObservableList()
		{
			_list = new List<T>();
		}

		public ObservableList(IEnumerable<T> items)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			_list = new List<T>(items);
		}

		public ObservableList(int capacity)
		{
			_list = new List<T>(capacity);
		} 

		#endregion

		#region Методы для работы с областями списка

		public void AddRange(IEnumerable<T> items)
		{
			InsertRange(_list.Count, items);
		}

		public void InsertRange(int index, IEnumerable<T> items)
		{
			_list.AddRange(items);
			OnItemAdded(index, items.ToArray());
		}

		public void RemoveRange(int index, int count)
		{
			var removedItems = _list.GetRange(index, count).ToArray();
			_list.RemoveRange(index, count);
			OnItemRemoved(index, removedItems);
		}

		public void ReplaceRange(int index, int count, IEnumerable<T> newItems)
		{
			var removedItems = _list.GetRange(index, count).ToArray();
			_list.RemoveRange(index, count);
			_list.InsertRange(index, newItems);
			OnItemsReplaced(index, removedItems, newItems.ToArray());
		} 

		#endregion

		#region IList<T> Members

		public int IndexOf(T item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			_list.Insert(index, item);
			OnItemAdded(index, item);
		}

		public void RemoveAt(int index)
		{
			var removedItem = _list[index];
			_list.RemoveAt(index);
			OnItemRemoved(index, removedItem);
		}

		public T this[int index]
		{
			get { return _list[index]; }
			set
			{
				var oldItem = _list[index];
				_list[index] = value;
				OnItemsReplaced(index, new[] { oldItem }, new[] { value });
			}
		}

		#endregion

		#region ICollection<T> Members

		public void Add(T item)
		{
			var index = _list.Count;
			_list.Add(item);
			OnItemAdded(index, item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly
		{
			get { return ((IList<T>)_list).IsReadOnly; }
		}

		public bool Remove(T item)
		{
			var index = _list.IndexOf(item);
			if (index > -1)
			{
				var removedItem = _list[index];
				_list.RemoveAt(index);
				OnItemRemoved(index, removedItem);
				return true;
			}
			return false;
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region Методы для оповещения об изменениях

		private void OnItemAdded(int index, params T[] addedItems)
		{
			OnChanged(ListChangeType.Insert, index, addedItems, new T[] { });
		}

		private void OnItemRemoved(int index, params T[] removedItems)
		{
			OnChanged(ListChangeType.Remove, index, new T[] { }, removedItems);
		}

		private void OnItemsReplaced(int index, T[] oldItems, T[] newItems)
		{
			OnChanged(ListChangeType.Replace, index, newItems, oldItems);
		}

		private void OnChanged(
			ListChangeType changeType,
			int changedSegmentStartIndex,
			T[] newSegmentItems,
			T[] oldSegmentItems)
		{
			OnChanged(
				new ListChangedEventArgs<T>(
					changeType,
					changedSegmentStartIndex,
					newSegmentItems,
					oldSegmentItems));
		} 

		protected virtual void OnChanged(ListChangedEventArgs<T> e)
		{
			if (Changed != null)
				Changed(this,e);
		}

		#endregion
	}

	public delegate void ListChangedEventHandler<T>(ObservableList<T> sender, ListChangedEventArgs<T> e);

	public class ListChangedEventArgs<T> : EventArgs
	{
		private readonly ListChangeType _changeType;
		private readonly int _changedSegmentStartIndex;
		private readonly T[] _newSegmentItems;
		private readonly T[] _oldSegmentItems;

		public ListChangedEventArgs(
			ListChangeType changeType,
			int changedSegmentStartIndex,
			T[] newSegmentItems,
			T[] oldSegmentItems)
		{
			_changeType = changeType;
			_oldSegmentItems = oldSegmentItems;
			_newSegmentItems = newSegmentItems;
			_changedSegmentStartIndex = changedSegmentStartIndex;
		}

		public T[] OldSegmentItems
		{
			get { return _oldSegmentItems; }
		}

		public T[] NewSegmentItems
		{
			get { return _newSegmentItems; }
		}

		public int ChangedSegmentStartIndex
		{
			get { return _changedSegmentStartIndex; }
		}

		public ListChangeType ChangeType
		{
			get { return _changeType; }
		}
	}

	public enum ListChangeType
	{
		Insert,
		Remove,
		Replace
	}
}