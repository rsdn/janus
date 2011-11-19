using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using JetBrains.Annotations;

using Rsdn.SmartApp;
using Rsdn.TreeGrid; //временно

namespace Rsdn.Janus
{
	public class NavigationTreeNode : INavigationTreeNode
	{
		private readonly string _name;
		private readonly INavigationItemHeader _header;
		private readonly string _navigationPageName;
		private readonly IList<INavigationTreeNode> _childrens;
		private readonly int _orderIndex;
		private NodeFlags _nodeFlags;

		public NavigationTreeNode(
			[NotNull] string name,
			[NotNull] INavigationItemHeader header,
			[NotNull] string navigationPageName,
			IEnumerable<INavigationTreeNode> childrens,
			int orderIndex)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (header == null)
				throw new ArgumentNullException("header");
			if (navigationPageName == null)
				throw new ArgumentNullException("navigationPageName");

			_name = name;
			_header = header;
			_orderIndex = orderIndex;
			_navigationPageName = navigationPageName;
			_childrens = (childrens ?? EmptyArray<INavigationTreeNode>.Value).ToArray().AsReadOnly();
		}

		#region Implementation of INavigationTreeNodeSource

		public string Name
		{
			get { return _name; }
		}

		#endregion

		#region Implementation of INavigationTreeNode

		public string NavigationPageName
		{
			get { return _navigationPageName; }
		}

		public INavigationItemHeader Header
		{
			get { return _header; }
		}

		public IList<INavigationTreeNode> Childrens
		{
			get { return _childrens; }
		}

		public int OrderIndex
		{
			get { return _orderIndex; }
		}

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

		int ICollection.Count
		{
			get { return Childrens.Count; }
		}

		object ICollection.SyncRoot
		{
			get { return this; }
		}

		bool ICollection.IsSynchronized
		{
			get { return true; }
		}

		ITreeNode ITreeNode.Parent
		{
			get { return null; }
		} //ToDo: возвращает null!!!

		NodeFlags ITreeNode.Flags
		{
			get { return Header.IsHighlighted ? _nodeFlags | NodeFlags.Highlight : _nodeFlags; }
			set { _nodeFlags = value; }
		}

		bool ITreeNode.HasChildren
		{
			get { return Childrens.Count > 0; }
		}

		ITreeNode ITreeNode.this[int iIndex]
		{
			get { return Childrens[iIndex]; }
		}

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