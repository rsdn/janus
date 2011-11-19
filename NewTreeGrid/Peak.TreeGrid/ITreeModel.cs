using System;
using System.Collections.Generic;
using System.Text;

namespace Peak.TreeGrid
{
	public interface ITreeModel<T>
	{
		// The root of the tree.
		T Root { get; }

		// The child of parent at index index in the parent's child array.
		IDualIndexable<T, int, T> Children { get; }

		//The number of children of parent.
		IIndexable<T, int> ChildCount { get; }

		// True if node is a leaf.
		IIndexable<T, bool> Leaf { get; }

		// returns IObjectInfo for given object
		IObjectInfo<T> GetObjectInfo(T Object);

		IIndexable<T, IObjectInfo<T>> ObjectInfos { get; }
	}
}
