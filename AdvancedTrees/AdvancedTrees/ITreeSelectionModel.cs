using System.Collections.Generic;

namespace AdvancedTrees
{
	public interface ITreeSelectionModel<T>
	{
		bool IsSelected(TreePath<T> path);
		void AddSelectionPaths(IEnumerable<TreePath<T>> paths);
		void RemoveSelectionPaths(IEnumerable<TreePath<T>> paths);
		void ClearSelection();

		//IObservable<> Changed { get; }
	}
}