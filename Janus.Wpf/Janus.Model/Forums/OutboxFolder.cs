using Janus.Model.TreeView;
using System.Collections.Generic;
using System.Linq;

namespace Janus.Model.Forums {
	public class OutboxFolder : ForumTreeNode<OutboxData> {
		public override ITreeNode this[int index] {
			get { return null; }
		}

		public override IEnumerable<ITreeNode> Children {
			get { return Enumerable.Empty<ITreeNode>(); }
		}

		public override int Count => 0;

		public override int Level => 0;
	}
}