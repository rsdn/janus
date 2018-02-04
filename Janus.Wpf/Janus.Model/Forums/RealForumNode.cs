using Janus.Model.TreeView;
using System.Collections.Generic;
using System.Linq;

namespace Janus.Model.Forums {
	public class RealForumNode : ForumTreeNode<RealForumData> {
		public override ITreeNode this[int index] => null;

		public override IEnumerable<ITreeNode> Children => Enumerable.Empty<ITreeNode>();

		public override int Count => 0;

		public override int Level => 1;
	}
}
