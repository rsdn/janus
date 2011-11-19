using System;

namespace AdvancedTrees
{
	public interface ITreeModel<T>
	{
		T RootParent { get; }

		IObservable<TreeModelChangedEventArgs<T>> Changed { get; }

		/// <summary>
		/// Returns the child of parent at index index in the parent's child array.
		/// </summary>
		T GetChild(T parent, int index);

		/// <summary>
		/// Returns the number of children of parent.
		/// </summary>
		int GetChildCount(T parent);

		/// <summary>
		/// Returns true if node is a leaf.
		/// </summary>
		bool IsLeaf(T node);
	}
}