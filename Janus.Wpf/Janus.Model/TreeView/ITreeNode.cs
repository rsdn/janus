using System.Collections.Generic;

namespace Janus.Model.TreeView {
	public interface ITreeNode {
		IEnumerable<ITreeNode> Children { get; }
		int Count { get; }
		int Level { get; }
		ITreeNode this[int index] { get; }
		object Data { get; set; }
		bool IsExpanded { get; set; }
		bool IsSelected { get; set; }
		ITreeNode Parent { get; }
	}

	public interface ITreeNode<TData> : ITreeNode {
		TData TypedData { get; set; }
	}
}
