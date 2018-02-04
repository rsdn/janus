namespace Janus.Model.Gui {
	public class WarningMessageEventArgs : MessageEventArgs {
		public WarningMessageEventArgs(string message, string title = "Warning") : base(message, title) {
		}
	}
}