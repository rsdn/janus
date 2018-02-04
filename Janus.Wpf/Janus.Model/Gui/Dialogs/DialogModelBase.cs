
namespace Janus.Model.Gui.Dialogs {
	public class DialogModelBase : ModelBase {
		protected string _Title;
		protected string _Message;
		protected ModelCommand _CloseCommand;

		public DialogModelBase() {
			_CloseCommand = new ModelCommand {
				DoExecute = _ => {
					UserInteraction.Modals.EndModal(this);
				},
			};
		}
		public DialogModelBase(string title, string message) : this() {
			_Title = title;
			_Message = message;
		}

		public string Title {
			get { return _Title; }
			set {
				if (string.Equals(_Title, value)) {
					return;
				}
				_Title = value;
				OnPropertyChanged();
			}
		}

		public string Message {
			get { return _Message; }
			set {
				if (string.Equals(_Message, value)) {
					return;
				}
				_Message = value;
				OnPropertyChanged();
			}
		}

		public ModelCommand CloseCommand {
			get { return _CloseCommand; }
			set { _CloseCommand = value; }
		}
	}
}