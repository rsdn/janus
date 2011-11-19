using System.Collections.Generic;

namespace AdvancedTrees
{
	public interface ITreeExpandModel<T>
	{
		bool IsExpanded(TreePath<T> path);
		void AddExpandedPaths(IEnumerable<TreePath<T>> paths);
		void RemoveExpandedPaths(IEnumerable<TreePath<T>> paths);
		void ClearExpandState();

		//IObservable<> Changed { get; }
	}
}