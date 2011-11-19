using System;

using Rsdn.SmartApp;

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
				"[{0:dd.MM.yyyy HH:mm:ss}] {1}".FormatStr(DateTime.Now, message);
			if (OnLog != null)
				OnLog(this, new LogEventArgs(this, new LogItem(et, LastMessage)));
		}
		#endregion

		public string LastMessage { get; private set; }
	}
}
