using System;
using System.Collections.Generic;
using System.Linq;

using Rsdn.Janus.AT;
using Rsdn.Janus.Framework;
using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	internal class MessagesSyncTask : RsdnSyncTask<ChangeRequest, ChangeResponse>
	{
		private const string _lastRatingRVName = "LastRatingRowVersion";
		private const string _lastForumRVName = "LastForumRowVersion";
		private const string _lastModerRVName = "LastModerRowVersion";

		public MessagesSyncTask(IServiceProvider provider, string name)
			: base(provider, name, () => Resources.QueryNewMessages)
		{}

		public override bool IsTaskActive()
		{
			return true;
		}

		private static IEnumerable<RequestForumInfo> GetSubscribedForums(
			IServiceProvider provider)
		{
			using (var mgr = provider.CreateDBContext())
			{
				var forums =
					mgr
						.SubscribedForums()
						.Select(
							f =>
								new RequestForumInfo
								{
									forumId = f.ID,
									isFirstRequest = f.LastSync < 0
								});
				foreach (var rq in forums)
					yield return rq;
			}
			yield return new RequestForumInfo { forumId = 0, isFirstRequest = false }; // trash 1
			yield return new RequestForumInfo { forumId = 58, isFirstRequest = false }; // trash 2
		}

		protected override ChangeRequest PrepareRequest(ISyncContext context)
		{
			var cfg = GetSyncConfig();
			var rq =
				new ChangeRequest
				{
					userName = cfg.Login,
					password = cfg.Password,
					ratingRowVersion = context.DBVars()[_lastRatingRVName].FromHexString(),
					messageRowVersion = context.DBVars()[_lastForumRVName].FromHexString(),
					moderateRowVersion = context.DBVars()[_lastModerRVName].FromHexString(),
					maxOutput = cfg.MaxMessagesPerSession,
					subscribedForums = GetSubscribedForums(context).ToArray()
				};
			return rq;
		}

		protected override ChangeResponse MakeRequest(ISyncContext context, JanusAT svc,
			ChangeRequest rq)
		{
			return svc.GetNewData(rq);
		}

		protected override void ProcessResponse(
			ISyncContext context,
			ChangeRequest request,
			ChangeResponse response)
		{
			SetSelfID(response.userId);
			MessagesSyncHelper.AddNewMessages(
				context,
				response.newMessages,
				response.newRating,
				response.newModerate,
				response.userId);
			context.DBVars()[_lastRatingRVName] = response.lastRatingRowVersion.ToHexString();
			context.DBVars()[_lastForumRVName] = response.lastForumRowVersion.ToHexString();
			context.DBVars()[_lastModerRVName] = response.lastModerateRowVersion.ToHexString();
		}
	}
}
