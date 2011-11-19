using System;

namespace Rsdn.Janus
{
	public class LogEventArgs : EventArgs
	{
		private readonly LogItem _item;
		private readonly object _sender;

		public LogEventArgs(object sender, LogItem item)
		{
			_sender = sender;
			_item = item;
		}

		public object Sender
		{
			get { return _sender; }
		}

		public LogItem Item
		{
			get { return _item; }
		}
	}
}