using Janus.Model.Forums;
using Janus.Model.Gui;

namespace Janus.Model {
	public class MainWindowModel : ModelBase {
		private readonly ForumTree _Forums = new ForumTree();
		private ModelCommand _ConfirmTest;

		public MainWindowModel() {
			_ConfirmTest = new ModelCommand() {
				DoCanExecute = _ => true,
				DoExecute = _ => UserInteraction.OnConfirmationMessage(this, "Confirm test"),
			};
		}

		public ForumTree Forums {
			get { return _Forums; }
		}

		public ModelCommand ConfirmTest { get => _ConfirmTest; }
	}
}