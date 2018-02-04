using Janus.Model.TreeView;

namespace Janus.Model.Forums {
	public interface IForumTreeNode<TData> : IForumTreeNode, ITreeNode<TData> where TData : ForumTreeData {
	}

	public interface IForumTreeNode : ITreeNode {

	}
}
