using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace AdvancedTrees
{
	public class TreeView : TreeView<object> { }

	public class TreeView<T> : Control
	{
		private const int _defaultLevelIndent = 20;

		private ITreeModel<T> _model = new EmptyTreeModel<T>();
		private IDisposable _modelChangedSubscription;
		private ITreeNodeRenderer<T> _nodeRenderer;
		private ITreeNodeMouseHandler<T> _nodeMouseHandler = new TreeNodeMouseHandler<T>();
		private ITreeExpandModel<T> _expandModel = new TreeExpandModel<T>();
		private ITreeSelectionModel<T> _selectionModel = new TreeSelectionModel<T>();
		private int _levelIndent = _defaultLevelIndent;
		private int _scrollOffset;
		private readonly List<TreePath<T>> _displayNodes = new List<TreePath<T>>();

		#region Public Members

		[NotNull]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITreeModel<T> Model
		{
			get { return _model; }
			set
			{
				if (_model == value)
					return;

				if (_modelChangedSubscription != null)
					_modelChangedSubscription.Dispose();

				_model = value;

				_modelChangedSubscription = _model.Changed.Subscribe(
					arg =>
					{
						var parentDisplayNodeIndex = _displayNodes.FindIndex(
							displayNode => displayNode.LastPathComponent.Equals(arg.Parent));
						if (parentDisplayNodeIndex < 0)
							return;
						var startIndex = parentDisplayNodeIndex + arg.ChildIndex;
						_displayNodes.RemoveRange(
							startIndex,
							arg.RemovedCount
								+ GetExpandedChildrenCount(parentDisplayNodeIndex, startIndex, arg.RemovedCount));
						_displayNodes.InsertRange(
							startIndex,
							CreateDisplayNodes(
								startIndex,
								_displayNodes[parentDisplayNodeIndex],
								arg.Parent,
								startIndex,
								arg.AddedCount));
					});

				FillRootNodes();
				Invalidate();
			}
		}

		[NotNull]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITreeExpandModel<T> ExpandModel
		{
			get { return _expandModel; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				_expandModel = value;

				FillRootNodes();
			}
		}

		[NotNull]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITreeSelectionModel<T> SelectionModel
		{
			get { return _selectionModel; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				_selectionModel = value;

				Invalidate();
			}
		}

		[NotNull]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITreeNodeRenderer<T> NodeRenderer
		{
			get { return _nodeRenderer; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				_nodeRenderer = value;

				Invalidate();
			}
		}

		[NotNull]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ITreeNodeMouseHandler<T> NodeMouseHandler
		{
			get { return _nodeMouseHandler; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				_nodeMouseHandler = value;
			}
		}

		[DefaultValue(_defaultLevelIndent)]
		public int LevelIndent
		{
			get { return _levelIndent; }
			set
			{
				_levelIndent = value;

				Invalidate();
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollOffset
		{
			get { return _scrollOffset; }
			set
			{
				if (value < 0 || value >= _displayNodes.Count)
					throw new ArgumentOutOfRangeException("value");

				_scrollOffset = value;

				Invalidate();
			}
		}

		#endregion

		#region Protected Members

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			DoubleBuffered = true;
			ResizeRedraw = true;

			_nodeRenderer = new SimpleTreeNodeRenderer<T>(Font, ForeColor, BackColor);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_modelChangedSubscription != null)
					_modelChangedSubscription.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var nodeRect = new Rectangle { Height = NodeRenderer.NodeHeight, Width = Width };
			var displayNodeIndex = ScrollOffset;
			while (nodeRect.Y < Width && displayNodeIndex < _displayNodes.Count)
			{
				var displayNode = _displayNodes[displayNodeIndex];
				nodeRect.X = (displayNode.Length - 1) * LevelIndent;
				NodeRenderer.DrawNode(
					e.Graphics,
					nodeRect,
					displayNode.LastPathComponent,
					SelectionModel.IsSelected(displayNode),
					Model.IsLeaf(displayNode.LastPathComponent),
					ExpandModel.IsExpanded(displayNode));
				nodeRect.Y += nodeRect.Height;
				displayNodeIndex++;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			var hitDisplayNodeIndex = HitTest(e.Location);
			if (hitDisplayNodeIndex < 0)
				return;

			Console.WriteLine(hitDisplayNodeIndex);

			MoveCursor(hitDisplayNodeIndex);
			NodeMouseHandler.MouseDown(
				_displayNodes[hitDisplayNodeIndex],
				GetNodeRectangle(hitDisplayNodeIndex),
				e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.Handled)
				return;

			switch (e.KeyData)
			{
				case Keys.Multiply:
					//expand with childs
					e.Handled = true;
					break;
				case Keys.Right:
					//expand or go to child
					e.Handled = true;
					break;
				case Keys.Left:
					//collapse or go to parent
					e.Handled = true;
					break;
				case Keys.Add:
					//Expand
					e.Handled = true;
					break;
				case Keys.Subtract:
					//Collapse
					e.Handled = true;
					break;
			}
		}

		#endregion

		#region Private Members

		private void FillRootNodes()
		{
			_scrollOffset = 0;
			_displayNodes.Clear();
			_displayNodes.AddRange(
				CreateDisplayNodes(
					0,  
					null,
					Model.RootParent,
					0, 
					Model.GetChildCount(Model.RootParent)));
		}

		private void ExpandDisplayNode(int displayNodeIndex)
		{
			var displayNode = _displayNodes[displayNodeIndex];
			if (Model.IsLeaf(displayNode.LastPathComponent) || ExpandModel.IsExpanded(displayNode))
				return;
			_displayNodes.InsertRange(
				displayNodeIndex + 1,
				CreateDisplayNodes(
					displayNodeIndex + 1,
					displayNode,
					displayNode.LastPathComponent,
					0,
					Model.GetChildCount(displayNode.LastPathComponent)));
		}

		private void CollapseDisplayNode(int displayNodeIndex)
		{
			var displayNode = _displayNodes[displayNodeIndex];
			if (Model.IsLeaf(displayNode.LastPathComponent) || !ExpandModel.IsExpanded(displayNode))
				return;
			_displayNodes.RemoveRange(
				displayNodeIndex + 1,
				GetExpandedChildrenCount(
					displayNodeIndex, 0, Model.GetChildCount(displayNode.LastPathComponent)));
		}

		private IEnumerable<TreePath<T>> CreateDisplayNodes(
			int insertIndex, TreePath<T> parentPath, T parent, int start, int count)
		{
			for (var i = insertIndex + start; i < insertIndex + start + count; i++)
			{
				var node = Model.GetChild(parent, i - insertIndex);
				var displayNode = parentPath!=null ? parentPath.Add(node) : new TreePath<T>(node);
				yield return displayNode;

				if (!Model.IsLeaf(node)) //&& ExpandModel.IsExpanded(parentDisplayNode))
					foreach (var childDisplayNode in 
							CreateDisplayNodes(i + 1, displayNode, node, 0, Model.GetChildCount(node)))
						yield return childDisplayNode;
			}
		}

		private int GetExpandedChildrenCount(int displayNodeIndex, int start, int count)
		{
			var displayNode = _displayNodes[displayNodeIndex];
			var expandedChildrenCount = Model.GetChildCount(displayNode.LastPathComponent);

			for (var i = displayNodeIndex + start; i < displayNodeIndex + start + count; i++)
			{
				var childDisplayNode = _displayNodes[i];
				if (!ExpandModel.IsExpanded(childDisplayNode))
					continue;

				var expandedCount = GetExpandedChildrenCount(
					i, 0, Model.GetChildCount(childDisplayNode.LastPathComponent));
				expandedChildrenCount += expandedCount;
				i += expandedCount;
			}

			return expandedChildrenCount;
		}

		private void MoveCursor(int displayNodeIndex)
		{
			//ToDo: implement
			if (!SelectionModel.IsSelected(_displayNodes[displayNodeIndex]))
				SelectionModel.AddSelectionPaths(new[] { _displayNodes[displayNodeIndex] });
			else
				SelectionModel.RemoveSelectionPaths(new[] { _displayNodes[displayNodeIndex] });

			if (!ExpandModel.IsExpanded(_displayNodes[displayNodeIndex]))
				ExpandModel.AddExpandedPaths(new[] { _displayNodes[displayNodeIndex] });
			else
				ExpandModel.RemoveExpandedPaths(new[] { _displayNodes[displayNodeIndex] });

			Invalidate();
		}

		private int HitTest(Point point)
		{
			var displayNodeIndex = (point.Y / NodeRenderer.NodeHeight) + ScrollOffset;
			return displayNodeIndex < _displayNodes.Count ? displayNodeIndex : -1;
		}

		private Rectangle GetNodeRectangle(int displayNodeIndex)
		{
			var displayNode = _displayNodes[displayNodeIndex];
			var x = (displayNodeIndex - ScrollOffset) * NodeRenderer.NodeHeight;
			return new Rectangle(
				x,
				(displayNode.Length - 1) * LevelIndent,
				Width - x,
				NodeRenderer.NodeHeight);
		}

		#endregion
	}
}