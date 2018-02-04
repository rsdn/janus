namespace Janus.Model.Gui {
	public class InformationMessageEventArgs : MessageEventArgs {
		public InformationMessageEventArgs(string message, string title = "Information") : base(message, title) {
		}
	}
}