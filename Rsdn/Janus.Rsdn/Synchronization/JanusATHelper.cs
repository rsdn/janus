using System;
using System.Linq;

using LinqToDB;

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

		internal static RequestForumInfo[] GetSubscribedForums(this IDataContext db)
		{
			return
				db
					.SubscribedForums()
					.Select(
						f =>
							new RequestForumInfo
							{
								forumId = f.ID,
								isFirstRequest = f.LastSync < 0
							})
					.ToArray();
		}
	}
}