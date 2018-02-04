using Janus.Model.Gui;
using Janus.Model.Gui.Dialogs;
using System;

namespace Janus.Wpf.Controls.DialogBoxes {
	public class ConfirmationModel : DialogModelBase {
		private ConfirmationButtons _Buttons = ConfirmationButtons.YesNo;
		private ConfirmationResult _Result = ConfirmationResult.Cancel;

		public ConfirmationModel() : this("", "") {

		}

		public ConfirmationModel(string title, string message,
			ConfirmationButtons buttons = ConfirmationButtons.YesNo) : base(title, message) {
			_Buttons = buttons;
			(_CloseCommand as ModelCommand).DoExecute = par => {
				if (Enum.TryParse<ConfirmationResult>(par as string, out var confirmRes)) {
					_Result = confirmRes;
					ModalPane.EndModal(this);
				}
				else {
					_Result = ConfirmationResult.Cancel;
					ModalPane.EndModal(this);
				}
			};
		}

		public ConfirmationButtons Buttons {
			get { return _Buttons; }
			set {
				if (_Buttons == value) {
					return;
				}
				_Buttons = value;
				OnPropertyChanged();
			}
		}

		public ConfirmationResult Result {
			get { return _Result; }
			set {
				if (_Result == value) {
					return;
				}
				_Result = value;
				OnPropertyChanged();
			}
		}
	}
}
