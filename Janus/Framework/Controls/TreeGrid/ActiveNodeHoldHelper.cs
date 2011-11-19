using System;
using System.Collections;

namespace Rsdn.TreeGrid
{
	/// <summary>
	/// Вспомогательный класс для удержания активного узла при обновлении коллекции.
	/// </summary>
	public class ActiveNodeHoldHelper : IDisposable
	{
		private readonly TreeGrid _grid;
		private readonly PathKey _pathKey;

		public ActiveNodeHoldHelper(TreeGrid grid)
		{
			_grid = grid;
			_pathKey = GetActivePathKey();
		}

		#region private impl
		/// <summary>
		/// Возвращает ключ пути указанной ноды.
		/// </summary>
		private static PathKey GetNodePathKey(ITreeNode node)
		{
			var al = new ArrayList();
			var curNode = node;
			while (curNode != null)
			{
				if (!(curNode is IKeyedNode))
					throw new InvalidOperationException(string.Format(
						"Type '{0}' must implement interface IKeyedNode to use path keys",
						curNode.GetType().FullName));
				al.Insert(0, ((IKeyedNode)curNode).Key);
				curNode = curNode.Parent;
			}
			return new PathKey((string[])al.ToArray(typeof (string)));
		}

		/// <summary>
		/// Возвращает ключ пути активной ноды или null если активной ноды нет.
		/// </summary>
		private PathKey GetActivePathKey()
		{
			if (_grid.ActiveNode == null)
				return null;
			return GetNodePathKey(_grid.ActiveNode);
		}

		private ITreeNode FindNodeByKey(ITreeNode parent, string key)
		{
			if (parent == null)
			{
				var rkn = _grid.Nodes as IKeyedNode;
				if ((rkn != null) && (rkn.Key == key))
					return _grid.Nodes;
				return null;
			}
			if (!parent.HasChildren)
				return null;
			foreach (ITreeNode tn in parent)
			{
				var kn = tn as IKeyedNode;
				if ((kn != null) && (kn.Key == key))
					return tn;
			}
			return null;
		}

		/// <summary>
		/// Возвращает ноду по указанному ключу пути или null если пути не существует.
		/// </summary>
		private ITreeNode FindNodeByPathKey(PathKey pathKey)
		{
			if ((_grid.Nodes == null) || (pathKey == null))
				return null;
			ITreeNode curNode = null;
			foreach (var key in pathKey.NodeKeys)
			{
				curNode = FindNodeByKey(curNode, key);
				if (curNode == null)
					return null;
			}
			return curNode;
		}

		private void SetActiveNodeByKey(PathKey pathKey)
		{
			_grid.ActiveNode = FindNodeByPathKey(pathKey);
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			SetActiveNodeByKey(_pathKey);
		}
		#endregion
	}
}