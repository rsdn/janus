using System;

using CodeJam.Services;

namespace Rsdn.Janus.Log
{
	/// <summary>
	/// Вспомогательные методы для работы с логом.
	/// </summary>
	public static class LogHelper
	{
		#region Новые методы
		public static void Log(
			this IServiceProvider provider,
			LogEventType eventType,
			string message)
		{
			var svc = provider.GetService<ILogger>();
			svc?.Log(eventType, message);
		}

		public static void LogInfo(this IServiceProvider provider, string message)
		{
			provider.Log(LogEventType.Information, message);
		}

		public static void LogWarning(this IServiceProvider provider, string message)
		{
			provider.Log(LogEventType.Warning, message);
		}

		public static void LogError(this IServiceProvider provider, string message)
		{
			provider.Log(LogEventType.Error, message);
		}
		#endregion

		#region Устаревшие методы со статическим контекстом
		public static void LogInfo(this ILogger logger, string message)
		{
			logger.Log(LogEventType.Information, message);
		}

		public static void LogWarning(this ILogger logger, string message)
		{
			logger.Log(LogEventType.Warning, message);
		}

		public static void LogError(this ILogger logger, string message)
		{
			logger.Log(LogEventType.Error, message);
		}
		#endregion
	}
}