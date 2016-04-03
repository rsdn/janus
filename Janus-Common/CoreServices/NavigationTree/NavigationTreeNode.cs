using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CodeJam.Collections;

using JetBrains.Annotations;

using Rsdn.TreeGrid; //временно

namespace Rsdn.Janus
{
	public class NavigationTreeNode : INavigationTreeNode
	{
		private NodeFlags _nodeFlags;

		public NavigationTreeNode(
			[NotNull] string name,
			[NotNull] INavigationItemHeader header,
			[NotNull] string navigationPageName,
			IEnumerable<INavigationTreeNode> childrens,
			int orderIndex)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (header == null)
				throw new ArgumentNullException(nameof(header));
			if (navigationPageName == null)
				throw new ArgumentNullException(nameof(navigationPageName));

			Name = name;
			Header = header;
			OrderIndex = orderIndex;
			NavigationPageName = navigationPageName;
			Childrens = (childrens ?? Array<INavigationTreeNode>.Empty).ToArray().AsReadOnly();
		}

		#region Implementation of INavigationTreeNodeSource

		public string Name { get; }
		#endregion

		#region Implementation of INavigationTreeNode

		public string NavigationPageName { get; }

		public INavigationItemHeader Header { get; }

		public IList<INavigationTreeNode> Childrens { get; }

		public int OrderIndex { get; }
		#endregion

		#region Implementation of IDropTarget

		public virtual void OnDragEnter(DragEventArgs e) { }

		public virtual void OnDragLeave(EventArgs e) { }

		public virtual void OnDragDrop(DragEventArgs e) { }

		public virtual void OnDragOver(DragEventArgs e) { }

		#endregion

		#region Implementation of ITreeNode (временно)

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Childrens.GetEnumerator();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			Childrens.CopyTo((INavigationTreeNode[])array, index);
		}

		int ICollection.Count => Childrens.Count;

		object ICollection.SyncRoot => this;

		bool ICollection.IsSynchronized => true;

		ITreeNode ITreeNode.Parent => null;
		//ToDo: возвращает null!!!

		NodeFlags ITreeNode.Flags
		{
			get { return Header.IsHighlighted ? _nodeFlags | NodeFlags.Highlight : _nodeFlags; }
			set { _nodeFlags = value; }
		}

		bool ITreeNode.HasChildren => Childrens.Count > 0;

		ITreeNode ITreeNode.this[int iIndex] => Childrens[iIndex];
		#endregion

		#region Implementation of IGetData

		public void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			nodeInfo.Highlight = Header.IsHighlighted;
			cellData[0].Text = Header.DisplayName;
			cellData[0].CellImageType = CellImageType.Image;
			cellData[0].Image = Header.Image;
			cellData[1].Text = Header.Info;
		}

		#endregion
	}
}