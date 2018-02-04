using System;

namespace Janus.Model.Gui {
	public class MessageEventArgs : EventArgs {
		public MessageEventArgs(string message, string title) {
			Title = title;
			Message = message;
		}

		public string Title { get; private set; }
		public string Message { get; private set; }
	}
}
