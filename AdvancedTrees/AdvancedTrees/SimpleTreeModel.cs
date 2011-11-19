using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace AdvancedTrees
{
	public class SimpleTreeModel<T> : ITreeModel<T>
	{
		private static readonly IObservable<TreeModelChangedEventArgs<T>> _changed =
			new Observable<TreeModelChangedEventArgs<T>>();

		private readonly IList<T> _list;
		private readonly T _rootParent;

		public SimpleTreeModel([NotNull] IList<T> list)
		{
			if (list == null)
				throw new ArgumentNullException("list");
			_list = list;
			_rootParent = default(T);
		}

		#region Implementation of ITreeModel<T>

		public T RootParent
		{
			get { return _rootParent; }
		}

		public IObservable<TreeModelChangedEventArgs<T>> Changed
		{
			get { return _changed; }
		}

		public T GetChild(T parent, int index)
		{
			if (Equals(parent, RootParent))
				return _list[index];
			throw new InvalidOperationException();
		}

		public int GetChildCount(T parent)
		{
			return _list.Count;
		}

		public bool IsLeaf(T node)
		{
			return !Equals(node, RootParent);
		}

		#endregion
	}
}