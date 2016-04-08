namespace Rsdn.Janus
{
	/// <summary>
	/// Элемент протокола.
	/// </summary>
	public class LogItem
	{
		public LogItem(LogEventType type, string message)
		{
			Type = type;
			Message = message;
		}

		public LogEventType Type { get; }

		public string Message { get; }
	}
}