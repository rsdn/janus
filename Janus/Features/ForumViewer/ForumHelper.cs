using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using LinqToDB;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Вспомогательные методы для работы с форумом.
	/// </summary>
	public static class ForumHelper
	{
		public static IJanusFormatter GetFormatter([NotNull] this IServiceProvider provider)
		{
			if (provider == null) throw new ArgumentNullException("provider");
			return provider.GetRequiredService<IJanusFormatter>();
		}

		public static string GetDisplayName(string name, string descript)
		{
			return Config.Instance.ForumDisplayConfig.ShowFullForumNames ? descript : name;
		}

		public static void MarkMsgRead(
			[NotNull] IServiceProvider provider,
			IEnumerable<MsgBase> messages,
			bool isRead,
			bool withSubnodes)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (messages == null || !messages.Any())
				return;

			var eventBroker = provider.GetRequiredService<IEventBroker>();

			var msgs =
				(withSubnodes
					? GetMarkedMsgList(messages, isRead).Distinct(MsgBaseComparer.Instance)
					: messages.Where(msg => msg.IsUnread == isRead))
				.ToArray();
			var msgIds = msgs
				.Select(msg => new ForumEntryIds(msg.ForumID, msg.TopicID, msg.ID))
				.ToArray();

			eventBroker.Fire(
				ForumEventNames.BeforeForumEntryChanged,
				new ForumEntryChangedEventArgs(msgIds, ForumEntryChangeType.ReadMark));

			provider
				.GetRequiredService<IMessageMarkService>()
				.QueueMessageMark(
					msgIds,
					isRead,
					() =>
					{
						// Обновляем агрегатную информацию об изменившихся форумах.
						var forums = msgs
							.Select(msg => msg.ForumID)
							.Distinct()
							.Select(id => Forums.Instance[id]);
						foreach (var forum in forums)
							forum.RefreshInfo();

						eventBroker.Fire(
							ForumEventNames.AfterForumEntryChanged,
							new ForumEntryChangedEventArgs(msgIds, ForumEntryChangeType.ReadMark));
					});

			foreach (var msg in msgs)
				if (msg.IsUnread == isRead)
					msg.SetUnread(!isRead);
		}

		public static void SetForumsReadMark(
			[NotNull] IServiceProvider provider,
			IEnumerable<IForum> forums,
			bool isRead)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (forums == null || !forums.Any())
				return;

			var eventBroker = provider.GetRequiredService<IEventBroker>();

			var forumsToMark = forums
				.Distinct()
				.Where(forum => forum.CanSetForumReadMark(isRead))
				.ToArray();
			var forumsIds = forumsToMark
				.Select(forum => new ForumEntryIds(forum.ID))
				.ToArray();

			eventBroker.Fire(
				ForumEventNames.BeforeForumEntryChanged,
				new ForumEntryChangedEventArgs(forumsIds, ForumEntryChangeType.ReadMark));

			foreach (Forum forum in forumsToMark)
			{
				DatabaseManager.MarkMessagesReadByForumId(provider, forum.ID, isRead);
				forum.Refresh();
			}

			eventBroker.Fire(
				ForumEventNames.AfterForumEntryChanged,
				new ForumEntryChangedEventArgs(forumsIds, ForumEntryChangeType.ReadMark));
		}

		private static int MarkOlderThan(
			IServiceProvider provider,
			DateTime? beforeDate,
			DateTime? afterDate,
			bool exceptAnswersMe,
			IEnumerable<int> forumsIds,
			bool asRead)
		{
			// Пометка сообщений. В два этапа. Сначала помечаем,
			// затем пересчёт агрегатов.
			//
			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				var q = db.Messages(m => m.IsRead != asRead);

				if (beforeDate.HasValue)
				{
					var beforeDateParam =
						new DateTime(
							beforeDate.Value.Year,
							beforeDate.Value.Month,
							beforeDate.Value.Day,
							beforeDate.Value.Hour,
							beforeDate.Value.Minute,
						// Если миллисекунды != 0, то вылетает исключение(OleDBException) - неверный тип, поэтому отрезаем их
							0);
					q = q.Where(m => m.Date < beforeDateParam);
				}

				if (afterDate.HasValue)
				{
					var afterDateParam =
						new DateTime(
							afterDate.Value.Year,
							afterDate.Value.Month,
							afterDate.Value.Day,
							afterDate.Value.Hour,
							afterDate.Value.Minute,
						// Если миллисекунды != 0, то вылетает исключение(OleDBException) - неверный тип, поэтому отрезаем их
							0);
					q = q.Where(m => m.Date > afterDateParam);
				}

				if (forumsIds.Any())
					q = q.Where(m => forumsIds.Contains(m.ForumID));

				if (exceptAnswersMe)
					q = q.Where(m => m.Parent.UserID == Config.Instance.SelfId);

				// Akasoft: Вот тут я удавился, добавляя параметром @uid. Пришлось заменить на String.Format(). 
				// 2 вечера отладки, полная фигня 2, ведь ниже-то всё работает...

				// Обновление агрегатов делаем в два этапа.
				// В начале просто сбрасываем все данные о прочтении в topic_info
				// для всех указанных форумов.
				// Затем смотрим в тех же форумах, есть ли в них непрочитанные сообщения
				// и сколько.
				// По полученным данным обновляем topic_info.
				var res =
					q
						.Set(_ => _.IsRead, () => asRead)
						.Update();
				if (res == 0)
					return 0;
				db
					.TopicInfos(ti => forumsIds.Contains(ti.ForumID))
						.Set(_ => _.AnswersUnread, 0)
						.Set(_ => _.AnswersToMeUnread, 0)
					.Update();

				var q1 = db.Messages(m => m.TopicID == 0);
				if (forumsIds.Any())
					q1 = q1.Where(m => forumsIds.Contains(m.ForumID));
				var q2 =
					from ma in q1
					join mb in
						(from im in db.Messages()
						 where im.TopicID != 0 && !im.IsRead
						 group im by im.TopicID
							 into grp
							 select new { TopicID = grp.Key, AnswersUnread = grp.Count() })
						on ma.ID equals mb.TopicID into ljb
					from mb in ljb
					join mc in
						(from im in db.Messages()
						 where
							 im.TopicID != 0
								 && !im.IsRead
								 && im.Parent.UserID == Config.Instance.SelfId
						 group im by im.TopicID
							 into grp
							 select new { TopicID = grp.Key, AnswersMeUnread = grp.Count() })
						on ma.ID equals mc.TopicID into ljc
					from mc in ljc
					where !ma.IsRead || mb.AnswersUnread > 0
					select new { ma.ID, mb.AnswersUnread, mc.AnswersMeUnread };
				var topics = q2.ToList();

				foreach (var item in topics)
				{
					var localItem = item;
					db
						.TopicInfos(ti => ti.MessageID == localItem.ID)
							.Set(_ => _.AnswersUnread, () => localItem.AnswersUnread)
							.Set(_ => _.AnswersToMeUnread, () => localItem.AnswersMeUnread)
						.Update();
				}

				tx.Commit();
				return res;
			}
		}

		public static int MarkMessagesByDate(
			[NotNull] IServiceProvider provider,
			IEnumerable<int> forumIds,
			bool isRead,
			DateTime? startDate,
			DateTime? endDate,
			bool exceptAnswersMe)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			var eventBroker = provider.GetRequiredService<IEventBroker>();

			var forumEntryIds = forumIds != null && forumIds.Any()
				? forumIds.Select(forumId => new ForumEntryIds(forumId)).ToArray()
				: new[] { ForumEntryIds.AllForums };

			eventBroker.Fire(
				ForumEventNames.BeforeForumEntryChanged,
				new ForumEntryChangedEventArgs(forumEntryIds, ForumEntryChangeType.ReadMark));

			var marksCount = 
				MarkOlderThan(
					provider,
					startDate,
					endDate,
					exceptAnswersMe,
					forumIds ?? Enumerable.Empty<int>(),
					isRead);

			(forumIds != null && forumIds.Any()
			 		? forumIds.Select(forumId => Forums.Instance[forumId])
			 		: Forums.Instance.ForumList)
				.ForEach(forum => forum.Refresh());

			eventBroker.Fire(
				ForumEventNames.AfterForumEntryChanged,
				new ForumEntryChangedEventArgs(forumEntryIds, ForumEntryChangeType.ReadMark));

			return marksCount;
		}

		/// <summary>
		/// Помечает сообщения до <paramref name="maxId"/> прочитанными,
		/// затем снимает флаг прочтения с укзанных сообщений.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="maxId">Максимальный ID сообщения, 
		/// который учавствует в выборке.</param>
		/// <param name="unreadIds">Массив ID сообщений, с которых следует 
		/// снять флаг прочитанности</param>
		private static void MarkMessagesRead(
			IServiceProvider provider,
			int maxId,
			IEnumerable<int> unreadIds)
		{
			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				var affectedIds = new HashSet<int>(
					db
						.Messages(m => m.ID < maxId + 1 && !m.IsRead)
						.Select(m => m.ID)
						.AsEnumerable()
						.Concat(unreadIds));

				// все сообщения до maxId прочитанныe
				db
					.Messages(m => !m.IsRead && m.ID < maxId + 1)
						.Set(_ => _.IsRead, true)
					.Update();

				// указанные сообщения не прочитанныe
				foreach (var series in unreadIds.SplitForInClause(provider))
				{
					var locSeries = series;
					db
						.Messages(m => locSeries.Contains(m.ID))
							.Set(_ => _.IsRead, false)
						.Update();
				}

				// обновляем агрегатную информацию
				DatabaseManager.UpdateTopicInfoSpecified(provider, db, affectedIds);

				tx.Commit();
			}
		}

		public static void MarkMessagesById(
			[NotNull] IServiceProvider provider,
			IEnumerable<int> unreadMsgIds,
			int maxId)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (unreadMsgIds == null || !unreadMsgIds.Any())
				return;

			var eventBroker = provider.GetRequiredService<IEventBroker>();
			var forumEntryIds = new[] { ForumEntryIds.AllForums };

			eventBroker.Fire(
				ForumEventNames.BeforeForumEntryChanged,
				new ForumEntryChangedEventArgs(forumEntryIds, ForumEntryChangeType.ReadMark));

			MarkMessagesRead(provider, maxId, unreadMsgIds);

			foreach (var forum in Forums.Instance.ForumList)
				forum.Refresh();

			eventBroker.Fire(
				ForumEventNames.AfterForumEntryChanged,
				new ForumEntryChangedEventArgs(forumEntryIds, ForumEntryChangeType.ReadMark));
		}

		private static void SetMsgGroupReadReplies(
			IServiceProvider provider,
			IEnumerable<int> mids,
			bool readReplies)
		{
			if (mids == null)
				throw new ArgumentNullException("mids");

			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				foreach (var ids in mids.SplitToSeries(provider.MaxInClauseElements()))
				{
					var localIds = ids;
					db
						.Messages(m => localIds.Contains(m.ID))
						.Set(_ => _.ReadReplies, readReplies)
						.Update();
				}
				tx.Commit();
			}
		}

		public static void SetMessageRepliesAutoReadMark(
			[NotNull] IServiceProvider provider,
			IEnumerable<MsgBase> messages,
			bool isEnabled)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (messages == null || !messages.Any())
				return;

			var eventBroker = provider.GetRequiredService<IEventBroker>();

			var msgs = GetReadRepliesMessages(messages, isEnabled, true).ToArray();
			var msgIds = msgs.Select(msg => new ForumEntryIds(msg.ForumID, msg.TopicID, msg.ID));

			eventBroker.Fire(
				ForumEventNames.BeforeForumEntryChanged,
				new ForumEntryChangedEventArgs(msgIds, ForumEntryChangeType.AutoReadMark));

			SetMsgGroupReadReplies(
				provider,
				msgs.Select(msg => msg.ID),
				isEnabled);

			eventBroker.Fire(
				ForumEventNames.AfterForumEntryChanged,
				new ForumEntryChangedEventArgs(msgIds, ForumEntryChangeType.AutoReadMark));
		}

		public static bool CanSetForumReadMark(this IForum forum, bool isRead)
		{
			return isRead ? forum.Unread > 0 : forum.Unread < forum.MessagesCount;
		}

		/// <summary>
		/// Ищем первое непрочитанное сообщение от текущего.
		/// Если ниже текущего нет непрочитанных, пытаемся искать от корня.
		/// </summary>
		/// <param name="forum"><see cref="Forum"/>.</param>
		/// <param name="skipActiveUnread">Флаг, указывающий стоит ли 
		/// пропускать текущее сообщение в заданном форуме, если оно не прочитано.</param>
		/// <returns>Непрочитанное сообщение <see cref="IMsg"/> 
		/// или <c>null, если сообщение не найдено.</c></returns>
		public static IMsg FindNextUnreadMsg(this Forum forum, bool skipActiveUnread)
		{
			return skipActiveUnread
				|| !(forum.ActiveMsg != null && forum.ActiveMsg.IsUnread)
				? forum.FindNextUnreadMsg()
				: forum.ActiveMsg;
		}

		/// <summary>
		/// Ищем первое непрочитанное сообщение от текущего.
		/// Если ниже текущего нет непрочитанных, пытаемся искать от корня.
		/// </summary>
		/// <param name="forum"><see cref="Forum"/>.</param>
		/// <returns>Непрочитанное сообщение <see cref="IMsg"/> 
		/// или <c>null, если сообщение не найдено.</c></returns>
		private static IMsg FindNextUnreadMsg(this Forum forum)
		{
			if (forum.Unread == 0 || forum.MessagesCount == 0)
				return null;

			Func<IMsg, bool> msgMatch = msg => msg.IsUnread;
			Func<IMsg, bool> topicMatch = msg => msg.RepliesUnread != 0 || msg.IsUnread;

			var activeMsg = forum.ActiveMsg;

			// Ищем от в текущем топике...
			if (activeMsg != null
				&& activeMsg.Topic.RepliesUnread != 0)
			{
				var message = activeMsg.Topic
					.Flatten(activeMsg).FirstOrDefault(msgMatch);

				if (message != null)
					return message;
			}

			// Доступные сообщения
			var root = forum.Msgs.Cast<IMsg>();

			// Если сообщения форума загружены не полностью, 
			// делаем две попытки поиска
			for (; ; )
			{
				// Создаем последовательность от текущего топика (не включая) до конца,
				// от начала до текущего, включительно
				var msgs =
					activeMsg == null
						? root
						: root
							.SkipWhile(msg => msg != activeMsg.Topic)
							.SkipWhile(msg => msg == activeMsg.Topic)
							.Concat(root.TakeWhile(msg => msg != activeMsg.Topic))
							.Concat(new[] { activeMsg.Topic });

				// Ищем топик...
				var message = msgs.FirstOrDefault(topicMatch);

				// Ищем непрочитанное сообщение...
				if (message != null)
					return message.Flatten().First(msgMatch);

				// если не нашли, но все сообщения загружены полностью
				if (forum.IsAllMsgLoaded)
					return null;

				// загружаем все сообщения и повторяем поиск
				root = forum.LoadAllMsg().Cast<IMsg>();
			}
		}

		public static IEnumerable<IMsg> Flatten(this IMsg root)
		{
			yield return root;

			foreach (IMsg msg in root)
				foreach (var child in Flatten(msg))
					yield return child;
		}

		private static IEnumerable<IMsg> Flatten(this IMsg root, IMsg startBeyond)
		{
			return Flatten(root).SkipWhile(msg => msg != startBeyond).Skip(1);
		}

		private static IEnumerable<MsgBase> GetMarkedMsgList(IEnumerable<MsgBase> msgs, bool markForRead)
		{
			foreach (Msg msg in msgs)
			{
				if (msg.IsUnread == markForRead)
					yield return msg;

				if (msg.HasChildren)
				{
					// Сколько у сообщения непрочитанных детей?
					var unreadedChildsCount = msg.RepliesUnread;
					// Минус себя
					if (msg.IsUnread)
						unreadedChildsCount--;

					// Все дочерние непрочитаны
					var allUnread = (msg.RepliesCount == unreadedChildsCount);
					// Все дочерние прочитаны
					var allRead = (unreadedChildsCount == 0);

					if (markForRead && !allRead || !markForRead && !allUnread)
						foreach (var child in GetMarkedMsgList(msg, markForRead))
							yield return child;
				}
			}
		}

		private static IEnumerable<MsgBase> GetReadRepliesMessages(
			IEnumerable<MsgBase> messages, bool readReplies, bool withSubnodes)
		{
			foreach (var message in messages)
			{
				if (message.ReadReplies != readReplies)
				{
					message.ReadReplies = readReplies;
					yield return message;
				}

				if (withSubnodes && message.HasChildren)
					foreach (var child in GetReadRepliesMessages(message, readReplies, true))
						yield return child;
			}
		}
	}
}
