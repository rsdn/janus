using System;

namespace Rsdn.Janus.AT
{
	/// <summary>
	/// Информация о сервисе JanusAT.
	/// </summary>
	public static class JanusATHelper
	{
		/// <summary>
		/// Синхронизировать форумы.
		/// </summary>
		public static void SyncForums(
			this IServiceProvider provider,
			SyncThreadPriority priority,
			SyncRequestFinishedHandler syncFinishedHandler,
			bool activateUI)
		{
			provider.SyncSpecific(
				JanusATInfo.ProviderName,
				JanusATInfo.ForumsSyncTaskName,
				priority,
				syncFinishedHandler,
				activateUI);
		}
	}
}