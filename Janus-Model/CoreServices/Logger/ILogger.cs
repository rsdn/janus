using System;

namespace Rsdn.Janus
{
	public interface ILogger
	{
		string LastMessage { get; }
		event EventHandler<LogEventArgs> OnLog;
		void Log(LogEventType et, string message);
	}
}