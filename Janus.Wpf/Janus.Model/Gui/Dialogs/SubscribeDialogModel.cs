using Janus.Model.Forums;
using Janus.Model.Forums.Persist;
using System.Linq;

namespace Janus.Model.Gui.Dialogs {
	public class SubscribeDialogModel : DialogModelBase {
		private readonly LightObservableCollection<RealForumNode> _Forums = new LightObservableCollection<RealForumNode>();

		public void LoadForums(IForumRepository repository) {
			_Forums.BeginUpdate();
			try {
				_Forums.Clear();
				_Forums.AddRange(repository.LoadForums(ForumRetreivalKind.All).Select(fd => new RealForumNode { TypedData = fd }));
			}
			finally {
				_Forums.EndUpdate();
			}
		}

		public LightObservableCollection<RealForumNode> Forums {
			get { return _Forums; }
		}
	}
}
