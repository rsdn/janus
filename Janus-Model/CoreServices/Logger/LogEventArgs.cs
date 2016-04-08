using System;

namespace Rsdn.Janus
{
	public class LogEventArgs : EventArgs
	{
		public LogEventArgs(object sender, LogItem item)
		{
			Sender = sender;
			Item = item;
		}

		public object Sender { get; }

		public LogItem Item { get; }
	}
}