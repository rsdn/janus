using System;

using Rsdn.SmartApp;

namespace AdvancedTrees
{
	public class EmptyTreeModel<T> : ITreeModel<T>
	{
		private static readonly IObservable<TreeModelChangedEventArgs<T>> _changed =
			new Observable<TreeModelChangedEventArgs<T>>();

		#region Implementation of ITreeModel<T>

		public T RootParent
		{
			get { return default(T); }
		}

		public IObservable<TreeModelChangedEventArgs<T>> Changed
		{
			get { return _changed; }
		}

		public T GetChild(T parent, int index)
		{
			throw new InvalidOperationException();
		}

		public int GetChildCount(T parent)
		{
			if (Equals(parent, RootParent))
				return 0;
			throw new InvalidOperationException();
		}

		public bool IsLeaf(T node)
		{
			if (Equals(node, RootParent))
				return true;
			throw new InvalidOperationException();
		}

		#endregion
	}
}