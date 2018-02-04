using Janus.Model.TreeView;
using System.Collections.Generic;
using System.Linq;

namespace Janus.Model.Forums {
	public class FavoritesFolder : ForumTreeNode<FavoritesData> {
		public override ITreeNode this[int index] => null;

		public override IEnumerable<ITreeNode> Children => Enumerable.Empty<ITreeNode>();

		public override int Count => 0;

		public override int Level => 0;
	}
}