namespace Janus.Model.Gui {
	public class ConfirmationMessageEventArgs : MessageEventArgs {
		public ConfirmationButtons Buttons { get; private set; } = ConfirmationButtons.YesNo;
		public ConfirmationResult Result { get; set; } = ConfirmationResult.Cancel;

		public ConfirmationMessageEventArgs(string message, string title = "Confirmation",
			ConfirmationButtons buttons = ConfirmationButtons.YesNo,
			ConfirmationResult result = ConfirmationResult.Cancel) : base(message, title) {
			Buttons = buttons;
			Result = result;
		}
	}
}