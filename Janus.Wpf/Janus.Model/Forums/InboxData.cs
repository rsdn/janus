using System.Collections.Generic;

namespace Janus.Model.Forums {
	public class InboxData : ForumTreeData {
		private readonly LightObservableCollection<RealForumData> _Forums = new LightObservableCollection<RealForumData>();
		public override string Title { get => "Inbox"; set { } }
		public override string ImageKey {
			get => "Inbox";
			set { }
		}

		public LightObservableCollection<RealForumData> Forums {
			get { return _Forums; }
		}

		public void AddForums(IEnumerable<RealForumData> enumerable) {
			_Forums.BeginUpdate();

			try {
				_Forums.Clear();
				_Forums.AddRange(enumerable);
			}
			finally {
				_Forums.EndUpdate();
			}
		}
	}
}