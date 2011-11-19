using System;
using System.Linq;
using System.Linq.Expressions;
using BLToolkit.Data.Linq;

namespace Rsdn.Janus
{
	// Сообщения
	static partial class DatabaseManager
	{
		public static T GetMessageByName<T>(
			IServiceProvider provider,
			string name,
			Expression<Func<DataModel.IForumMessage, T>> resultSelector)
		{
			using (var dbMgr = provider.CreateDBContext())
				return
					dbMgr
						.Messages(msg => msg.Name == name)
						.Select(resultSelector)
						.SingleOrDefault();
		}

		public static int? FindMessageForumId(IServiceProvider provider, int msgId)
		{
			using (var mgr = provider.CreateDBContext())
				return
					mgr
						.Messages(msg => msg.ID == msgId)
						.Select(msg => new int?(msg.ForumID))
						.SingleOrDefault();
		}

		public static string GetMessageBody(IServiceProvider provider, int mid)
		{
			using (var mgr = provider.CreateDBContext())
				return
					mgr
						.Messages(msg => msg.ID == mid)
						.Select(msg => msg.Message)
						.Single();
		}

		public static Msg GetMessageWithForum(IServiceProvider provider, int mid)
		{
			using (var db = provider.CreateDBContext())
				return
					db
						.Message(
							mid,
							m =>
								new Msg(provider)
								{
									ID = m.ID,
									Date = m.Date,
									UserID = m.UserID,
									IsRead = m.IsRead,
									IsMarked = m.IsMarked,
									Subject = m.Subject,
									Name = m.Name,
									UserNick = m.UserNick.ToUserDisplayName(m.UserClass),
									UserClass = (short)m.UserClass,
									ArticleId = m.ArticleId.GetValueOrDefault(),
									Rating = m.Rating(),
									Smiles = m.SmileCount(),
									Agrees = m.AgreeCount(),
									Disagrees = m.DisagreeCount(),
								});
		}

		public static void SetMessageMarked(
			IServiceProvider provider,
			MsgBase msg,
			bool isMarked)
		{
			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				db
					.Messages(m => m.ID == msg.ID)
						.Set(_ => _.IsMarked, isMarked)
					.Update();

				UpdateTopicInfo(db, msg.TopicID);

				tx.Commit();
			}
		}

		public static void MarkMessagesReadByForumId(
			IServiceProvider provider,
			int forumId,
			bool isRead)
		{
			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				if (isRead)
				{
					db
						.Messages(m => !m.IsRead && m.ForumID == forumId)
							.Set(_ => _.IsRead, true)
						.Update();
					db
						.TopicInfos(ti => ti.ForumID == forumId)
							.Set(_ => _.AnswersUnread,     0)
							.Set(_ => _.AnswersToMeUnread, 0)
						.Update();
				}
				else
				{
					db
						.Messages(m => m.ForumID == forumId)
							.Set(_ => _.IsRead, false)
						.Update();
					UpdateTopicInfoByFilter(
						db,
						ti => ti.ForumID == forumId,
						m => m.ForumID == forumId);
				}

				tx.Commit();
			}
		}
	}
}
