using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Extensibility.EventBroker;
using CodeJam.Services;
using CodeJam.Strings;

using JetBrains.Annotations;

using LinqToDB;

using Rsdn.Janus.DataModel;
using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	public static class ForumsSubscriptionHelper
	{
		public static void UpdateForumPriority(
			IServiceProvider provider,
			int forumId,
			int newPriority)
		{
			using (var db = provider.CreateDBContext())
				db
					.SubscribedForums(f => f.ID == forumId)
						.Set(_ => _.Priority, newPriority)
					.Update();
		}

		public static IQueryable<T> GetAllForums<T>(
			IDataContext mgr,
			Expression<Func<IServerForum, T>> srvSelector,
			Expression<Func<ISubscribedForum, T>> subsSelector)
		{
			var subsFrms = mgr.SubscribedForums();
			return
				mgr
					.ServerForums()
					.Where(f => !subsFrms.Any(of => of.ID == f.ID))
					.Select(srvSelector)
					.Union(subsFrms.Select(subsSelector));
		}

		public static void UpdateForumsSubscriptions(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] IEnumerable<ForumSubscriptionRequest> requests,
			bool promptToDelete)
		{
			ProgressWorker.Run(
				serviceProvider,
				false,
				progressVisualizer =>
				{
					progressVisualizer.SetProgressText(Resources.SubscribeMessage);
					UpdateForumsSubscriptions(
						serviceProvider,
						requests,
						(id, name, descript) =>
							promptToDelete
								&& MessageBox.Show(
									Resources.WarningOnUnsubscribeForum.FormatWith(descript),
									Resources.WarningOnUnsubscribeForumCaption,
									MessageBoxButtons.YesNo,
									MessageBoxIcon.Question) == DialogResult.Yes);
				});
		}

		private delegate bool DeleteMessagesPredicate(int id, string name, string descript);

		private static void Subscribe(IServiceProvider provider, int fid)
		{
			using (var db = provider.CreateDBContext())
				db
					.ServerForums(f => f.ID == fid)
					.Into(db.SubscribedForums())
						.Value(_ => _.ID, f => f.ID)
						.Value(_ => _.Name, f => f.Name)
						.Value(_ => _.Descript, f => f.Descript)
						.Value(_ => _.LastSync, f => -1)
					.Insert();
		}

		private static void Unsubscribe(
			IServiceProvider provider,
			int fid,
			bool deleteMessages)
		{
			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				if (deleteMessages)
				{
					db.TopicInfos(ti => ti.ForumID == fid).Delete();
					db.Messages(m => m.ForumID == fid).Delete();
				}

				//За подробностями: http://rsdn.ru/Forum/Message.aspx?mid=2808495&only=1

				//db.Execute(@"
				//	DELETE FROM [favorites]
				//	WHERE
				//		[favorites].[mid] IN
				//			(SELECT [mid] FROM [messages]
				//				WHERE [messages].[gid] = @gid)",
				//	"@gid", fid);

				db.SubscribedForums(f => f.ID == fid).Delete();
				tx.Commit();
			}
		}

		private static void UpdateForumsSubscriptions(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] IEnumerable<ForumSubscriptionRequest> requests,
			[NotNull] DeleteMessagesPredicate deleteMessagesPredicate)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (requests == null)
				throw new ArgumentNullException(nameof(requests));
			if (deleteMessagesPredicate == null)
				throw new ArgumentNullException(nameof(deleteMessagesPredicate));

			using (var mgr = serviceProvider.CreateDBContext())
			{
				var forums =
					GetAllForums(
							mgr,
							f => new {f.ID, f.Name, f.Descript, Subscribed = false},
							f => new {f.ID, f.Name, f.Descript, Subscribed = true})
						.ToDictionary(forum => forum.ID);
				var requestsArray =
					requests
						.Where(request => request.IsSubscribed != forums[request.ForumId].Subscribed)
						.ToArray();
				if (!requestsArray.Any())
					return;
				var forumEntryIds =
					requestsArray
						.Select(request => new ForumEntryIds(request.ForumId))
						.ToArray();
				var eventBroker = serviceProvider.GetRequiredService<IEventBroker>();

				eventBroker.Fire(
					ForumEventNames.BeforeForumEntryChanged,
					new ForumEntryChangedEventArgs(forumEntryIds, ForumEntryChangeType.ForumSubscription));

				foreach (var request in requestsArray)
					if (request.IsSubscribed)
						Subscribe(serviceProvider, request.ForumId);
					else
					{
						var forum = forums[request.ForumId];

						Unsubscribe(
							serviceProvider,
							request.ForumId,
							deleteMessagesPredicate(forum.ID, forum.Name, forum.Descript));
					}
				serviceProvider
					.GetRequiredService<IUIShell>()
					.CreateUIAsyncOperation()
					.PostOperationCompleted(() => serviceProvider.GetRequiredService<IUIShell>().RefreshData());

				eventBroker.Fire(
					ForumEventNames.AfterForumEntryChanged,
					new ForumEntryChangedEventArgs(forumEntryIds, ForumEntryChangeType.ForumSubscription));
			}
		}

		public static bool IsTrashForum(int forumId)
		{
			return forumId == 0 || forumId == 58;
		}
	}
}