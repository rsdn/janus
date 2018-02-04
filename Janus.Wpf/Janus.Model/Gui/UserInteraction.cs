using Janus.Model.Gui.Dialogs;
using System;

namespace Janus.Model.Gui {
	public static class UserInteraction {
		public static event EventHandler<ErrorMessageEventArgs> ErrorMessage;
		public static void OnErrorMessage(object sender, string message, string title = "Error") {
			ErrorMessage?.Invoke(sender, new ErrorMessageEventArgs(message, title));
		}

		public static event EventHandler<WarningMessageEventArgs> WarningMessage;

		public static void OnWarningMessage(object sender, string message, string title = "Warning") {
			WarningMessage?.Invoke(sender, new WarningMessageEventArgs(message, title));
		}

		public static event EventHandler<InformationMessageEventArgs> InformationMessage;
		public static void OnInformationMessage(object sender, string message, string title = "Information") {
			InformationMessage?.Invoke(sender, new InformationMessageEventArgs(message, title));
		}

		public static event EventHandler<ConfirmationMessageEventArgs> ConfirmationMessage;

		public static ConfirmationResult OnConfirmationMessage(object sender, string message, string title = "Confirmation",
			ConfirmationButtons buttons = ConfirmationButtons.OkCancel) {
			var args = new ConfirmationMessageEventArgs(message, title, buttons);
			ConfirmationMessage?.Invoke(sender, args);
			return args.Result;
		}

		public static class Modals {
			public static Action<DialogModelBase> OnShowModal;
			public static Action<DialogModelBase> OnEndodal;

			public static void EndModal(DialogModelBase dialogModelBase) {
				OnEndodal?.Invoke(dialogModelBase);
			}

			public static void ShowModal(DialogModelBase dialogModelBase) {
				OnShowModal?.Invoke(dialogModelBase);
			}
		}

	}
}
