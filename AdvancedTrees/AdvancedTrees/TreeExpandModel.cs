using System.Collections.Generic;

namespace AdvancedTrees
{
	public class TreeExpandModel<T> : ITreeExpandModel<T>
	{
		private readonly HashSet<TreePath<T>> _expandedPaths = new HashSet<TreePath<T>>();

		#region Implementation of ITreeExpandModel<T>

		public bool IsExpanded(TreePath<T> path)
		{
			return _expandedPaths.Contains(path);
		}

		public void AddExpandedPaths(IEnumerable<TreePath<T>> paths)
		{
			foreach (var path in paths)
				_expandedPaths.Add(path);
		}

		public void RemoveExpandedPaths(IEnumerable<TreePath<T>> paths)
		{
			foreach (var path in paths)
				_expandedPaths.Remove(path);
		}

		public void ClearExpandState()
		{
			_expandedPaths.Clear();
		}

		#endregion
	}
}