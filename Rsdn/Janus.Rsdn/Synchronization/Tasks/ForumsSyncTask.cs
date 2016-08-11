using System;
using System.Collections.Generic;
using System.Linq;

using LinqToDB;

using Rsdn.Janus.AT;
using Rsdn.Janus.Framework;
using Rsdn.Janus.org.rsdn;
using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	internal class ForumsSyncTask : RsdnSyncTask<ForumRequest, ForumResponse>
	{
		private const string _lastForumListRowVersion = "LastForumListRowVersion";

		public ForumsSyncTask(IServiceProvider provider, string name)
			: base(provider, name, () => Resources.GetForumsList)
		{
		}

		public override bool IsTaskActive()
		{
			return true;
		}

		protected override ForumRequest PrepareRequest(ISyncContext context)
		{
			var cfg = GetSyncConfig();
			return
				new ForumRequest
				{
					userName = cfg.Login,
					password = cfg.Password,
					forumsRowVersion = context.DBVars()[_lastForumListRowVersion].FromHexString(),
				};
		}

		protected override ForumResponse MakeRequest(ISyncContext context, JanusAT svc, ForumRequest rq)
		{
			return svc.GetForumList(rq);
		}

		/// <summary>
		/// Синхронизация списка форумов - AT.
		/// </summary>
		/// <returns>количество новых форумов</returns>
		/// <remarks>Существенное отличие - по новой технологии
		/// происходит инкрементальное обновление списка.
		/// В связи с этим форумы не удаляются (впрочем, и прецедента такого пока не было)</remarks>
		private static int SyncServerForums(
			IDataContext db,
			IEnumerable<JanusForumInfo> forums)
		{
			// Find intersecting ids
			var ids = forums.Select(f => f.forumId).ToArray();
			var intersect =
				new HashSet<int>(
					db
						.ServerForums()
						.Select(f => f.ID)
						.Where(id => ids.Contains(id)));

			var newForums = 0;
			foreach (var forum in forums)
			{
				var locForum = forum;
				// Дублирование кода из-за того, что джет не понимает именованных параметров
				if (intersect.Contains(forum.forumId))
				{
					db
						.ServerForums(f => f.ID == locForum.forumId)
								 .Set(_ => _.Name,      locForum.shortForumName)
								 .Set(_ => _.Descript,  locForum.forumName)
								 .Set(_ => _.Rated,     locForum.rated != 0)
								 .Set(_ => _.InTop,     locForum.inTop != 0)
								 .Set(_ => _.RateLimit, locForum.rateLimit)
						.Update();

					db
						.SubscribedForums(f => f.ID == locForum.forumId)
									 .Set(_ => _.Name,     locForum.shortForumName)
									 .Set(_ => _.Descript, locForum.forumName)
						.Update();
				}
				else
				{
					db
						.ServerForums()
							.Value(_ => _.ID,        locForum.forumId)
							.Value(_ => _.Name,      locForum.shortForumName)
							.Value(_ => _.Descript,  locForum.forumName)
							.Value(_ => _.Rated,     locForum.rated != 0)
							.Value(_ => _.InTop,     locForum.inTop != 0)
							.Value(_ => _.RateLimit, locForum.rateLimit)
						.Insert();
					newForums++;
				}
			}
			return newForums;
		}

		protected override void ProcessResponse(
			ISyncContext context,
			ForumRequest request,
			ForumResponse response)
		{
			var newForums = 0;
			if (response.forumList.Length > 0)
				using (var db = context.CreateDBContext())
				using (var tx = db.BeginTransaction())
				{
					newForums = SyncServerForums(db, response.forumList);
					tx.Commit();
				}
			context.DBVars()[_lastForumListRowVersion] = response.forumsRowVersion.ToHexString();
			context.StatisticsContainer.AddValue(JanusATInfo.ForumsStats, response.forumList.Length);
			if (newForums > 0)
				context.StatisticsContainer.AddValue(JanusATInfo.NewForumsStat, newForums);
		}
	}
}
