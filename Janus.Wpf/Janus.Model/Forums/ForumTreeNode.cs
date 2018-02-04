using Janus.Model.TreeView;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Janus.Model.Forums {
	public abstract class ForumTreeNode<TData> : ForumTreeNode, IForumTreeNode<TData> where TData : ForumTreeData {
		private TData _Data;

		public ForumTreeNode() {
			if (typeof(TData).GetTypeInfo().GetConstructor(new Type[0]) != null) {
				TypedData = Activator.CreateInstance<TData>();
			}
		}

		public TData TypedData {
			get { return _Data; }
			set {
				if (_Data == value) {
					return;
				}
				_Data = value;
				OnPropertyChanged();
			}
		}

		public override object Data {
			get { return TypedData; }
			set { TypedData = value as TData; }
		}

		public override bool IsExpanded {
			get { return TypedData.IsExpanded ?? Level > 0; }
			set {
				TypedData.IsExpanded = (value == Level > 0) ? (bool?)null : value;
				OnPropertyChanged();
			}
		}

		public override bool IsSelected {
			get { return TypedData.IsSelected; }
			set {
				TypedData.IsSelected = value;
				if (value) {
					var parent = (this as ITreeNode).Parent;
					while (parent != null) {
						parent.IsExpanded = true;
						parent = parent.Parent;
					}
				}
				OnPropertyChanged();
			}
		}

	}

	public abstract class ForumTreeNode : ModelBase, IForumTreeNode {
		private ForumTreeNode _Parent;

		public ForumTreeNode Parent {
			get { return _Parent; }
			set {
				if (_Parent == value) {
					return;
				}
				_Parent = value;
				OnPropertyChanged();
			}
		}

		ITreeNode ITreeNode.Parent {
			get { return Parent; }
		}


		#region abstract interface implementation
		public abstract ITreeNode this[int index] { get; }
		public abstract IEnumerable<ITreeNode> Children { get; }
		public abstract int Count { get; }
		public abstract int Level { get; }
		public abstract object Data { get; set; }
		public abstract bool IsExpanded { get; set; }
		public abstract bool IsSelected { get; set; }
		#endregion
	}
}
