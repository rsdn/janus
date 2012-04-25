using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Rsdn.TreeGrid;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Коллекция ITreeNode, учитывающая их древовидное устройство.
	/// </summary>
	public class TreeNodeBag : Collection<ITreeNode>
	{
		public TreeNodeBag()
		{}

		public TreeNodeBag(IEnumerable<ITreeNode> nodes)
		{
			AddRange(nodes);
		}

		public void AddRange(IEnumerable<ITreeNode> nodes)
		{
			if (nodes == null)
				throw new ArgumentNullException("nodes");

			foreach (var node in nodes)
				Add(node);
		}

		#region Tree support
		public static ITreeNode DeepFind(ITreeNode where, ITreeNode find)
		{
			if (where == null)
				throw new ArgumentNullException("where");
			if (find == null)
				throw new ArgumentNullException("find");

			if (find == where)
				return where;

			foreach (ITreeNode node in where)
			{
				if (node == find)
					return node;

				var check = DeepFind(node, find);
				if (check != null)
					return check;
			} //for

			return null;
		}

		public ITreeNode DeepFind(ITreeNode find)
		{
			if (find == null)
				throw new ArgumentNullException("find");

			return
				Items
					.Select(node => DeepFind(node, find))
					.FirstOrDefault(check => check != null);
		}

		public bool ContainsRecursion(ITreeNode label)
		{
			if (label == null)
				throw new ArgumentNullException("label");

			if (Items.Any(node => node.Parent == label))
				return true;

			for (var node = label; node != null; node = node.Parent)
				if (DeepFind(node) != null)
					return true;

			return false;
		}

		public bool IsAssignableFrom(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			return Items.All(type.IsInstanceOfType);
		}
		#endregion Tree support
	}
}