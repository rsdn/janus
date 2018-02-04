namespace Janus.Model.Gui {
	public class ErrorMessageEventArgs : MessageEventArgs {
		public ErrorMessageEventArgs(string message, string title = "Error") : base(message, title) { }
	}
}