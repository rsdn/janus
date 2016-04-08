using System;
using System.Collections.Generic;
using System.Linq;

using CodeJam.Services;

using LinqToDB;

using Rsdn.Janus.AT;
using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	/// <summary>
	/// Задача отсылки сообщений - AT.
	/// </summary>
	internal class PostMessagesSyncTask : RsdnSyncTask<PostRequest, PostResponse>
	{
		public PostMessagesSyncTask(IServiceProvider provider, string name)
			: base(provider, name, () => Resources.PostMessages)
		{}

		public override bool IsTaskActive()
		{
			var obMgr = Provider.GetRequiredService<IOutboxManager>();
			return
				obMgr
					.NewMessages
					.FirstOrDefault(msg => !msg.Hold) != null
				|| obMgr.RateMarks.Count > 0;
		}

		protected override PostRequest PrepareRequest(ISyncContext context)
		{
			using (var db = context.CreateDBContext())
			{
				var writed =
					db
						.OutboxMessages(m => !m.Hold)
						.Select(
							m =>
								new PostMessageInfo
								{
									forumId = m.ForumID,
									localMessageId = m.ID,
									message = m.Body
										+ (string.IsNullOrEmpty(m.Tagline)
											? ""
											: $"\r\n[tagline]{m.Tagline}[/tagline]"),
									parentId = m.ReplyToID,
									subject = m.Subject
								})
						.ToArray();
				var rates =
					db
						.OutboxRates()
						.Select(
							r =>
								new PostRatingInfo
									{
										localRatingId = r.ID,
										messageId = r.MessageID,
										rate = (int)r.RateType
									})
						.ToArray();
				var cfg = GetSyncConfig();
				return
					new PostRequest
						{
							userName = cfg.Login,
							password = cfg.Password,
							writedMessages = writed,
							rates = rates
						};
			}
		}

		protected override PostResponse MakeRequest(ISyncContext context, JanusAT svc, PostRequest rq)
		{
			svc.PostChange(rq);
			context.CheckState();
			return svc.PostChangeCommit();
		}

		private static void ClearOutbox(IServiceProvider provider, IEnumerable<int> mids)
		{
			var max = provider.MaxInClauseElements();
			using (var db = provider.CreateDBContext())
				foreach (var series in mids.SplitToSeries(max))
					// ReSharper disable AccessToForEachVariableInClosure
					db.OutboxMessages(m => series.Contains(m.ID)).Delete();
					// ReSharper restore AccessToForEachVariableInClosure
		}

		private static void ClearRatesOutbox(IServiceProvider provider)
		{
			using (var db = provider.CreateDBContext())
				db
					.OutboxRates()
					.Delete();
		}

		protected override void ProcessResponse(
			ISyncContext context,
			PostRequest request,
			PostResponse response)
		{
			ClearOutbox(context, response.commitedIds);
			ClearRatesOutbox(context);

			context.StatisticsContainer.AddValue(JanusATInfo.OutboundMessagesStats, response.commitedIds.Length);
			context.StatisticsContainer.AddValue(JanusATInfo.FailedOutboundMessagesStats, response.exceptions.Length);
			context.StatisticsContainer.AddValue(JanusATInfo.OutboundRatesStats, request.rates.Length);

			foreach (var ex in response.exceptions)
				context.TryAddSyncError(
					new SyncErrorInfo(SyncErrorType.CriticalError, GetDisplayName(), ex.exception));

			// TODO: Нужна какая то механика, отображающая исключения для каждого сообщения в UI исходящих
		}
	}
}
