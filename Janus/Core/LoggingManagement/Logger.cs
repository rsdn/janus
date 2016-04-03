using System;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	/// <summary>
	/// Класс для ведения истории и хинтов.
	/// </summary>
	[Service(typeof (ILogger))]
	public class Logger : ILogger
	{
		#region ILogger Members
		public event System.EventHandler<LogEventArgs> OnLog;

		public void Log(LogEventType et, string message)
		{
			LastMessage =
				$"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {message}";
			OnLog?.Invoke(this, new LogEventArgs(this, new LogItem(et, LastMessage)));
		}
		#endregion

		public string LastMessage { get; private set; }
	}
}
