using System;
using System.Collections.Generic;
using System.Linq;

using CodeJam;
using CodeJam.Collections;
using CodeJam.Services;

using LinqToDB;

using Rsdn.Janus.DataModel;
using Rsdn.Janus.Log;
using Rsdn.Janus.org.rsdn;
using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	// Вспомогательный класс для сохранения новых сообщений в БД
	internal static class MessagesSyncHelper
	{
		private static readonly Func<IDataContext, JanusMessageInfo, bool, bool, DateTime?, int> _addMsgQuery =
			CompiledQuery.Compile<IDataContext, JanusMessageInfo, bool, bool, DateTime?, int>(
				(db, msg, isRead, markRead, lastModerated) =>
					db
						.Messages()
						.Value(_ => _.ID, msg.messageId)
						.Value(_ => _.ForumID, msg.forumId)
						.Value(_ => _.TopicID, msg.topicId)
						.Value(_ => _.ParentID, msg.parentId)
						.Value(_ => _.Date, msg.messageDate)
						.Value(_ => _.UserNick, msg.userNick)
						.Value(_ => _.Subject, msg.subject)
						.Value(_ => _.Message, msg.message)
						.Value(_ => _.UserClass, ToJanusUserClass(msg.userRole))
						.Value(_ => _.IsRead, isRead)
						.Value(_ => _.UserID, msg.userId)
						.Value(_ => _.ArticleId, msg.articleId)
						.Value(_ => _.ReadReplies, markRead)
						.Value(_ => _.Name, msg.messageName)
						.Value(_ => _.LastModerated, lastModerated)
						.Value(_ => _.Closed, msg.closed)
						.Insert());

		private static readonly Func<IDataContext, JanusMessageInfo, DateTime?, int> _updateMsgQuery =
			CompiledQuery.Compile<IDataContext, JanusMessageInfo, DateTime?, int>(
				(db, msg, lastModerated) =>
					db
						.Messages(m => m.ID == msg.messageId)
						.Set(_ => _.ForumID, msg.forumId)
						.Set(_ => _.TopicID, msg.topicId)
						.Set(_ => _.ParentID, msg.parentId)
						.Set(_ => _.Date, msg.messageDate)
						.Set(_ => _.UserID, msg.userId)
						.Set(_ => _.UserNick, msg.userNick)
						.Set(_ => _.Subject, msg.subject)
						.Set(_ => _.Message, msg.message)
						.Set(_ => _.UserClass, ToJanusUserClass(msg.userRole))
						.Set(_ => _.ArticleId, msg.articleId)
						.Set(_ => _.LastModerated, lastModerated)
						.Set(_ => _.Name, msg.messageName)
						.Set(_ => _.Closed, msg.closed)
						.Update());

		private static readonly Func<IDataContext, int, int, int> _insertTagQuery =
			CompiledQuery.Compile<IDataContext, int, int, int>(
				(db, msgId, tagId) =>
					db
						.MessageTags()
						.Value(_ => _.MessageID, msgId)
						.Value(_ => _.TagID, tagId)
						.Insert());

		private static readonly Func<IDataContext, int, int> _delTagsQuery =
			CompiledQuery.Compile<IDataContext, int, int>(
				(db, msgId) =>
					db
						.MessageTags(mt => mt.MessageID == msgId)
						.Delete());

		private static readonly Func<IDataContext, int, int, int> _deleteRatesQuery =
			CompiledQuery.Compile<IDataContext, int, int, int>(
				(db, msgId, userId) =>
					db
						.Rates(r => r.MessageID == msgId && r.UserID == userId)
						.Delete());

		private static readonly Func<IDataContext, JanusRatingInfo, MessageRates, int> _insertRateQuery =
			CompiledQuery.Compile<IDataContext, JanusRatingInfo, MessageRates, int>(
				(db, rate, rateType) =>
					db
						.GetTable<IRate>()
						.Value(_ => _.MessageID, rate.messageId)
						.Value(_ => _.TopicID, rate.topicId)
						.Value(_ => _.UserID, rate.userId)
						.Value(_ => _.RateType, rateType)
						.Value(_ => _.Multiplier, rate.userRating)
						.Value(_ => _.Date, rate.rateDate)
						.Insert());

		private static readonly Func<IDataContext, JanusRatingInfo, MessageRates, MessageRates, int> _updateRatesQuery =
			CompiledQuery.Compile<IDataContext, JanusRatingInfo, MessageRates, MessageRates, int>(
				(db, rate, rateType, oldRateType) =>
					db
						.GetTable<IRate>()
						.Where(
							r =>
								r.MessageID == rate.messageId
								&& r.UserID == rate.userId
								&& r.RateType == oldRateType)
						.Set(_ => _.TopicID, () => rate.topicId)
						.Set(_ => _.RateType, () => rateType)
						.Set(_ => _.Multiplier, () => rate.userRating)
						.Set(_ => _.Date, () => rate.rateDate)
						.Update());

		public static void AddNewMessages(
			ISyncContext context,
			JanusMessageInfo[] messages,
			JanusRatingInfo[] rates,
			JanusModerateInfo[] moderatorials,
			int selfID)
		{
			AddNewMessages(
				context,
				messages,
				rates,
				moderatorials,
				null,
				selfID);
		}

		private static void AddNewRates(
			IDataContext db,
			JanusRatingInfo[] rates,
			Action<int, int> progressHandler)
		{
			var processed = 0;

			foreach (var rate in rates)
			{
				// Могут встречаться случаи (+/- && :) && оценка). +, - и :)
				// выражаются отрицательными константами. Так же может приходить
				// команда на удаление всего рейтинга.

				var localRate = rate;
				var rateType = (MessageRates) rate.rate;
				if (rateType == MessageRates.DeleteRate)
				{
					_deleteRatesQuery(db, rate.messageId, rate.userId);
					continue;
				}

				var q =
					db
						.Rates(r => r.MessageID == localRate.messageId && r.UserID == localRate.userId);
				if (rateType == MessageRates.Agree || rateType == MessageRates.DisAgree)
					q = q.Where(r => r.RateType == MessageRates.Agree || r.RateType == MessageRates.DisAgree);
				else if (rateType <= 0)
					q = q.Where(r => r.RateType == rateType);
				else
					q = q.Where(r => r.RateType > MessageRates.DisAgree);

				var oldRate =
					q
						.Select(r => (MessageRates?)r.RateType)
						.SingleOrDefault();
				var newRate = rate;
				if (!oldRate.HasValue)
					_insertRateQuery(db, newRate, rateType);
				else
					_updateRatesQuery(db, newRate, rateType, oldRate.Value);

				processed++;
				progressHandler?.Invoke(rates.Length, processed);
			}
		}

		private static bool IsModeratorialExists(IDataContext db, int messageId, int userId)
		{
			return
				db
					.Moderatorials()
					.Count(m => m.MessageID == messageId && m.UserID == userId) > 0;
		}

		private static void AddModeratorials(
			IDataContext db,
			IEnumerable<JanusModerateInfo> moderatorials)
		{
			foreach (var mdr in moderatorials)
			{
				var localMdr = mdr;
				if (IsModeratorialExists(db, mdr.messageId, mdr.userId))
					db
						.Moderatorials(
							mod =>
							mod.MessageID == localMdr.messageId
							&& mod.UserID == localMdr.userId)
						.Set(_ => _.Create, () => localMdr.create)
						.Set(_ => _.ForumID, () => localMdr.forumId)
						.Update();
				else
					db
						.Moderatorials()
						.Value(_ => _.MessageID, localMdr.messageId)
						.Value(_ => _.UserID, localMdr.userId)
						.Value(_ => _.ForumID, localMdr.forumId)
						.Value(_ => _.Create, localMdr.create)
						.Insert();
			}
		}

		public static void AddNewMessages(
			ISyncContext context,
			JanusMessageInfo[] messages,
			JanusRatingInfo[] rates,
			JanusModerateInfo[] moderatorials,
			Action<IDataContext> afterProcessInTxHandler,
			int selfID)
		{
			if (messages == null)
				throw new ArgumentNullException(nameof(messages));
			if (rates == null)
				throw new ArgumentNullException(nameof(rates));
			if (messages.Length == 0 && rates.Length == 0)
				// Nothing to do
				return;

			context.LogInfo(Resources.ProcessMessages);

			var msgIds = Array<int>.Empty;
			// Затычка. Блокируем интерфейс на время обработки сообщений.
			using (context.GetRequiredService<IUIShell>().FreezeUI(context))
			using (context.GetRequiredService<IJanusDatabaseManager>().GetLock().GetWriterLock())
			{
				var dbMgr = context.GetRequiredService<IJanusDatabaseManager>();
				using (var db = dbMgr.CreateDBContext())
				using (var tx = db.BeginTransaction())
				{
					var tids = new HashSet<int>();
					var pgSvc = context.GetService<ISyncProgressVisualizer>();

					var topicIds = Array<int>.Empty;
					if (messages.Length > 0)
						AddMessages(
							context,
							db,
							messages,
							selfID,
							pgSvc != null
								? (total, current) =>
									{
										pgSvc.ReportProgress(total, current);
										pgSvc.SetProgressText(
											current
												.GetDeclension(
													Resources.NewMsgProcessingProgressText1,
													Resources.NewMsgProcessingProgressText2,
													Resources.NewMsgProcessingProgressText5)
												.FormatWith(current));
									}
								: (Action<int, int>) null,
							out topicIds,
							out msgIds);
					foreach (var msg in messages)
						tids.Add(msg.topicId != 0 ? msg.topicId : msg.messageId);
					foreach (var id in topicIds)
						tids.Add(id);

					AddNewRates(
						db,
						rates,
						pgSvc != null
							? (total, current) =>
								{
									pgSvc.ReportProgress(total, current);
									pgSvc.SetProgressText(
										current
											.GetDeclension(
												Resources.NewRatesProcessingProgress1,
												Resources.NewRatesProcessingProgress2,
												Resources.NewRatesProcessingProgress5)
											.FormatWith(current));
								}
							: (Action<int, int>) null);
					foreach (var rate in rates)
						tids.Add(rate.topicId != 0 ? rate.topicId : rate.messageId);

					AddModeratorials(db, moderatorials);

					// Вариант с получением топиков с сервера
					foreach (var tid in moderatorials.Select(
						mod => mod.topicId == 0 ? mod.messageId : mod.topicId))
						tids.Add(tid);

					context.GetRequiredService<IRsdnForumService>().UpdateForumAggregates(context, db, tids);

					afterProcessInTxHandler?.Invoke(db);

					tx.Commit();
					GC.KeepAlive(db);
				}
			}

			AddBrokenTopicRequests(context, messages);

			var searchSvc = context.GetRequiredService<ISearchService>();
			var addedCount =
				searchSvc.AddMessagesToIndex(
					messages.Select(
						svcMsg =>
							new MessageSearchInfo(
								svcMsg.messageId,
								svcMsg.messageDate,
								svcMsg.subject,
								svcMsg.message,
								svcMsg.forumId,
								svcMsg.userId,
								svcMsg.userNick)));
			context.StatisticsContainer.AddValue(JanusATInfo.IndexedMessagesStats, addedCount);

			context.LogInfo(
				Resources
					.DownloadTopicsStat
					.FormatWith(
						msgIds.Length,
						msgIds.Length.GetDeclension(
							Resources.Messages1,
							Resources.Messages2,
							Resources.Messages5)));

			context.StatisticsContainer.AddValue(JanusATInfo.MessagesStats, messages.Length);
			context.StatisticsContainer.AddValue(JanusATInfo.RatesStats, rates.Length);
			context.StatisticsContainer.AddValue(JanusATInfo.ModeratorialsStats, moderatorials.Length);
		}

		private static void AddBrokenTopicRequests(
			IServiceProvider provider,
			IEnumerable<JanusMessageInfo> messages)
		{
			var freshMids = new Dictionary<int, JanusMessageInfo>();
			var parentIds = new Dictionary<int, int>();
			// Собираем все messageId и parentId
			foreach (var msg in messages)
			{
				freshMids[msg.messageId] = msg;
				if (msg.parentId != 0 && !ForumsSubscriptionHelper.IsTrashForum(msg.forumId)) // Skip roots & trash
					parentIds[msg.parentId] = msg.messageId;
			}

			// Удаляем тех родителей, которые уже присутствуют в том же пакете
			foreach (var parentId in parentIds.Keys.ToArray())
				if (freshMids.ContainsKey(parentId))
					parentIds.Remove(parentId);

			using (var dbMgr = provider.CreateDBContext())
			{
				var ids =
					dbMgr
						.Messages()
						.Where(msg => parentIds.Keys.ToArray().Contains(msg.ID))
						.Select(msg => msg.ID);
				// Удаляем тех родителей, которые присутствуют в БД
				foreach (var mid in ids)
					parentIds.Remove(mid);
			}

			// Оборванных веток нет - выходим
			if (parentIds.Count <= 0)
				return;

			provider.LogInfo(
				string.Format(
					parentIds.Count.GetDeclension(
						Resources.BrokenTopicMessage1,
						Resources.BrokenTopicMessage2,
						Resources.BrokenTopicMessage5),
					parentIds.Count,
					parentIds.Values.JoinToStringSeries(30).FirstOrDefault()));
			// Добавляем оставшиеся в запросы топиков
			// TODO: вменяемые строковые константы
			foreach (var mid in parentIds.Values)
				provider
					.GetOutboxManager()
					.DownloadTopics
					.Add(
						Resources.BrokenTopicRequestSource,
						mid,
						Resources.BrokenTopicRequestHint.FormatWith(freshMids[mid].subject));
		}

		/// <summary>
		/// Добавление сообшений полученыых с сервера в базу
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="db"></param>
		/// <param name="messages">Добавляемые сообщения</param>
		/// <param name="selfid"></param>
		/// <param name="progressHandler">обработчик прогресса обработки сообщений</param>
		/// <param name="updatedTopicIds">Какие топики были обновлены.</param>
		/// <param name="updatedMessageIds">Какие сообщения были добавлены.</param>
		private static void AddMessages(
			[JetBrains.Annotations.NotNull] IServiceProvider provider,
			IDataContext db,
			JanusMessageInfo[] messages,
			int selfid,
			Action<int, int> progressHandler,
			out int[] updatedTopicIds,
			out int[] updatedMessageIds)
		{
			if (db == null)
				throw new ArgumentNullException(nameof(db));
			if (messages == null)
				throw new ArgumentNullException(nameof(messages));

			var msgIds = new List<int>();
			var topicIds = new List<int>();
			var processed = 0;
			var tagCache =
				LazyDictionary.Create<string, int>(
					tag =>
					{
						var id =
							db
								.Tags(t => t.TagValue == tag)
								.Select(t => (int?)t.ID)
								.FirstOrDefault()
							?? Convert.ToInt32(
								db
									.Tags()
									.Value(_ => _.TagValue, tag)
									.InsertWithIdentity());
						return id;
					},
					false);
			foreach (var msg in messages)
			{
				if (ForumsSubscriptionHelper.IsTrashForum(msg.forumId))
					try
					{
						var mid = msg.messageId;
						db.TopicInfos(ti => ti.MessageID == mid).Delete();
						db.Messages(m => m.ID == mid).Delete();
					}
					catch (Exception)
					{
						provider.LogError(Resources.ErrorOnMessageDelete + msg.messageId);
					}
				else
				{
					var updatedTid = db.Message(msg.messageId, m => (int?) m.TopicID);

					if (msg.message.IsNullOrEmpty())
						msg.message = "<none>";

					var lastModerated =
						msg.lastModerated == DateTime.MinValue
							? null
							: (DateTime?) msg.lastModerated;
					try
					{
						if (!updatedTid.HasValue)
						{
							var markRead = false;
							if (msg.parentId != 0)
								markRead = db.Message(msg.parentId, m => m.ReadReplies);

							var isRead =
								msg.userId == selfid
									? provider.GetRequiredService<IRsdnSyncConfigService>().GetConfig().MarkOwnMessages
									: markRead;

							_addMsgQuery(db, msg, isRead, markRead, lastModerated);

							msgIds.Add(msg.messageId);
						}
						else
						{
							_updateMsgQuery(db, msg, lastModerated);
							_delTagsQuery(db, msg.messageId);
							topicIds.Add(updatedTid.Value == 0 ? msg.messageId : updatedTid.Value);
						}
						if (msg.tags != null)
							foreach (var tag in msg.tags)
								_insertTagQuery(db, msg.messageId, tagCache[tag]);
					}
					catch (Exception e)
					{
						// Какая ....!
						provider.LogError($"{Resources.ErrorOnMessageProcessing}{msg.messageId} : {e.Message}");
					}
				}

				processed++;
				progressHandler?.Invoke(messages.Length, processed);
			}

			db
				.SubscribedForums()
				.Set(_ => _.LastSync, 1)
				.Update();
			updatedTopicIds = topicIds.ToArray();
			updatedMessageIds = msgIds.ToArray();
		}

		/// <summary>
		/// Ремап ролей веб-сервиса в классы януса.
		/// </summary>
		/// <remarks>
		/// Необходимо так как wsdl.exe теряет всю информацию о числовом значении
		/// перечислений и просто перенумеровывает их по новому.
		/// </remarks>
		private static UserClass ToJanusUserClass(UserRole role)
		{
			switch (role)
			{
				case UserRole.Admin:
					return UserClass.Admin;
				case UserRole.Moderator:
					return UserClass.Moderator;
				case UserRole.TeamMember:
					return UserClass.Team;
				case UserRole.User:
					return UserClass.User;
				case UserRole.Expert:
					return UserClass.Expert;
				case UserRole.Group:
					return UserClass.Group;
				default:
					return UserClass.Anonym;
			}
		}
	}
}