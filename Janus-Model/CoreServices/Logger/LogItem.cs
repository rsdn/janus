namespace Rsdn.Janus
{
	/// <summary>
	/// Элемент протокола.
	/// </summary>
	public class LogItem
	{
		private readonly string _message;
		private readonly LogEventType _type;

		public LogItem(LogEventType type, string message)
		{
			_type = type;
			_message = message;
		}

		public LogEventType Type
		{
			get { return _type; }
		}

		public string Message
		{
			get { return _message; }
		}
	}
}