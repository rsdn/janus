using Janus.Model.TreeView;
using System.Collections.Generic;
using System.Linq;

namespace Janus.Model.Forums {
	public class InboxFolder : ForumTreeNode<InboxData> {
		private LightObservableCollection<RealForumNode> _Forums = new LightObservableCollection<RealForumNode>();

		public override ITreeNode this[int index] => _Forums[index];

		public override IEnumerable<ITreeNode> Children => _Forums;

		public override int Count => _Forums.Count;

		public override int Level => 0;

		public void AddForums(IEnumerable<RealForumData> enumerable) {
			TypedData.AddForums(enumerable);
			_Forums.BeginUpdate();
			try {
				_Forums.Clear();
				_Forums.AddRange(TypedData.Forums.Select(forumData => new RealForumNode { TypedData = forumData }));
			}
			finally {
				_Forums.EndUpdate();
			}
			OnPropertyChanged(nameof(Count));
			OnPropertyChanged(nameof(Children));
			OnPropertyChanged("Items");
		}
	}
}