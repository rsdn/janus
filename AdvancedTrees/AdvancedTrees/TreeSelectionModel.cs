using System.Collections.Generic;

namespace AdvancedTrees
{
	public class TreeSelectionModel<T> : ITreeSelectionModel<T>
	{
		private readonly HashSet<TreePath<T>> _srlectedPaths = new HashSet<TreePath<T>>();
		
		#region Implementation of ITreeSelectionModel<T>

		public bool IsSelected(TreePath<T> path)
		{
			return _srlectedPaths.Contains(path);
		}

		public void AddSelectionPaths(IEnumerable<TreePath<T>> paths)
		{
			foreach (var path in paths)
				_srlectedPaths.Add(path);
		}

		public void RemoveSelectionPaths(IEnumerable<TreePath<T>> paths)
		{
			foreach (var path in paths)
				_srlectedPaths.Remove(path);
		}

		public void ClearSelection()
		{
			_srlectedPaths.Clear();
		}

		#endregion
	}
}