using Janus.Model.TreeView;
using System.Collections.Generic;

namespace Janus.Model.Forums {
	public class ForumTree : ForumTreeNode<ForumTreeData> {
		private OutboxFolder _Outbox = new OutboxFolder();
		private InboxFolder _Inbox = new InboxFolder();
		private SearchFolder _Search = new SearchFolder();
		private FavoritesFolder _Favorites = new FavoritesFolder();

		public ForumTree() {
			TypedData.Title = "Forums";
		}

		public override ITreeNode this[int index] {
			get {
				switch (index) {
					case 0:
						return _Outbox;
					case 1:
						return _Inbox;
					case 2:
						return _Search;
					case 3:
						return _Favorites;
					default:
						return null;
				}
			}
		}

		public override IEnumerable<ITreeNode> Children => new ITreeNode[] { _Outbox, _Inbox, _Search, _Favorites };

		public override int Count => 4;

		public override int Level => -1;

		public OutboxFolder Outbox => _Outbox;
		public InboxFolder Inbox => _Inbox;
		public SearchFolder Search => _Search;
		public FavoritesFolder Favorites => _Favorites;
	}
}
