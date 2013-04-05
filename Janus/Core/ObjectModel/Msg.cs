using System;
using System.Collections.Generic;
using System.Linq;

using Rsdn.Janus.DataModel;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сообщение форума.
	/// </summary>
	public class Msg : MsgBase
	{
		private const int _frameSize = 200;

		private bool _partiallyLoaded;
		private int _loadIndex;

		public Msg(IServiceProvider provider) : base(provider)
		{}

		protected override IMsg GetTopic()
		{
			IMsg curr = this;

			while (curr.Parent.Parent != null)
				curr = curr.Parent;

			return curr;
		}

		protected override int GetTopicId()
		{
			return GetTopic().ID;
		}

		#region IGetData - реализация интерфейса.
		protected override void GetDataExt(CellInfo[] aryCellData)
		{
			aryCellData[ExtInfoColumn].Text = GetFormattedRatingForReplies();
		}

		public override void GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			if (_partiallyLoaded)
				LoadFrame(this);
			base.GetData(nodeInfo, cellData);
		}
		#endregion

		#region Работа с данными.
		// Заполняет дочерние ветки. Производит дочитку.
		protected override void FillChildren()
		{
			RepliesCount      = 0;
			RepliesToMeUnread = 0;
			RepliesUnread     = 0;
			RepliesMarked     = 0;

			if (Children != null)
				Children.Clear();

			//DatabaseManager.GetChildList(this);
			GetTopicChildren(ServiceProvider);
		}

		private void GetTopicChildren(IServiceProvider provider)
		{
			using (var db = provider.CreateDBContext())
			{
				var q = db.Messages(m => m.TopicID == ID).OrderBy(m => m.ParentID);
				q =
					Config.Instance.ForumDisplayConfig.NewMessagesOnTop
						? q.ThenByDescending(m => m.Date)
						: q.ThenBy(m => m.Date);
				var msgs =
					q
						.Select(
							m =>
								new Msg(provider)
								{
									Parent = this,
									IsChild = true,

									ID = m.ID,
									ParentID = m.ParentID,
									ForumID = m.ForumID,
									Name = m.Name,
									Date = m.Date,
									Subject = m.Subject,
									UserID = m.UserID,
									UserClass = (short)m.UserClass,
									UserNick = m.UserNick,
									IsRead = m.IsRead,
									IsMarked = m.IsMarked,
									Closed = m.Closed,
									ReadReplies = m.ReadReplies,
									LastModerated = m.LastModerated,
									ArticleId = m.ArticleId,
									Rating = m.Rating(),
									Smiles = m.SmileCount(),
									Agrees = m.AgreeCount(),
									Disagrees = m.DisagreeCount(),
									Moderatorials = m.ActiveModeratorialCount()
								})
					.Cast<MsgBase>()
					.ToList();
				foreach (var msg in msgs)
					msg.EndMapping();
			}
		}

		public static IMsg FilterFirstLevel(
			IServiceProvider provider,
			Msg curMsg,
			string text)
		{
			var msg = new Msg(provider);
			var children =
				curMsg
					.Children
					.Where(
						msgBase =>
							msgBase
								.Subject
								.ToLower()
								.IndexOf(text.ToLower()) != -1)
					.ToList();

			msg.Children = children;

			return msg;
		}
		#endregion

		#region Загрузка топиков - кейсет.
		public override NodeFlags Flags
		{
			get
			{
				if (_partiallyLoaded)
					LoadFrame(this);
				return base.Flags;
			}
			set { base.Flags = value; }
		}

		public override bool HasChildren
		{
			get
			{
				if (_partiallyLoaded)
					LoadFrame(this);
				return base.HasChildren;
			}
		}

		#region Keyset experiment
		//private static IQueryable<int> GetKeysetSource(
		//  IDataContext db,
		//  int forumID,
		//  SortType sort)
		//{
		//  if (sort == SortType.ByLastUpdateDateAsc || sort == SortType.ByLastUpdateDateDesc)
		//  {
		//    var topicSrc = db.TopicInfos(ti => ti.ForumID == forumID);
		//    topicSrc =
		//      sort == SortType.ByLastUpdateDateAsc
		//        ? topicSrc.OrderBy(ti => ti.LastUpdateDate)
		//        : topicSrc.OrderByDescending(ti => ti.LastUpdateDate);
		//    return topicSrc.Select(ti => ti.MessageID);
		//  }
		//  var msgSrc = db.Messages(m => m.TopicID == 0 && m.ForumID == forumID);
		//  switch (sort)
		//  {
		//    case SortType.ByDateAsc:
		//      msgSrc = msgSrc.OrderBy(m => m.Date);
		//      break;
		//    case SortType.ByDateDesc:
		//      msgSrc = msgSrc.OrderByDescending(m => m.Date);
		//      break;
		//    case SortType.ByIdAsc:
		//      msgSrc = msgSrc.OrderBy(m => m.ID);
		//      break;
		//    case SortType.ByIdDesc:
		//      msgSrc = msgSrc.OrderByDescending(m => m.ID);
		//      break;
		//    case SortType.BySubjectAsc:
		//      msgSrc = msgSrc.OrderBy(m => m.Subject);
		//      break;
		//    case SortType.BySubjectDesc:
		//      msgSrc = msgSrc.OrderByDescending(m => m.Subject);
		//      break;
		//    case SortType.ByAuthorAsc:
		//      msgSrc = msgSrc.OrderBy(m => m.UserNick);
		//      break;
		//    case SortType.ByAuthorDesc:
		//      msgSrc = msgSrc.OrderByDescending(m => m.UserNick);
		//      break;
		//    case SortType.ByForumAsc:
		//      msgSrc = msgSrc.OrderBy(m => m.ServerForum.Name);
		//      break;
		//    case SortType.ByForumDesc:
		//      msgSrc = msgSrc.OrderByDescending(m => m.ServerForum.Name);
		//      break;
		//    default:
		//      throw new ArgumentOutOfRangeException("sort");
		//  }
		//  return msgSrc.Select(m => m.ID);
		//}

		//public static Msg GetTopicsKeyset(
		//  IServiceProvider provider,
		//  int forumID,
		//  SortType sort,
		//  bool isLoadAll)
		//{
		//  var virtualRoot = new Msg(provider);
		//  using (var db = provider.CreateDBManager())
		//  {
		//    var src = GetKeysetSource(db, forumID, sort);
		//    var displayConfig = Config.Instance.ForumDisplayConfig;
		//    if (!(isLoadAll || displayConfig.MaxTopicsPerForum <= 0))
		//      src = src.Take(displayConfig.MaxTopicsPerForum);
		//    virtualRoot.Children =
		//      src
		//        .Select(
		//          id =>
		//            new Msg(provider)
		//            {
		//              ID = id,
		//              ParentID = 0,
		//              ForumID = forumID,
		//              Parent = virtualRoot,
		//              IsChild = false,
		//              _partiallyLoaded = true
		//            })
		//        .Cast<MsgBase>()
		//        .ToList();
		//    var idx = 0;
		//    foreach (Msg msg in virtualRoot.Children)
		//    {
		//      msg._loadIndex = idx;
		//      idx++;
		//    }
		//  }
		//  return virtualRoot;
		//}

		#endregion


		private static void LoadFrame(Msg msg)
		{
			var children = ((Msg) msg.Parent).Children;
			var frameTop = msg._loadIndex / _frameSize * _frameSize;
			var ids =
				Enumerable
					.Range(frameTop, Math.Min(_frameSize, children.Count - frameTop))
					.Select(i => children[i].ID)
					.ToArray();
			var provider = msg.ServiceProvider;
			using (var db = provider.CreateDBContext())
			{
				var msgs =
					db
						.TopicInfos(ti => ids.Contains(ti.MessageID))
						.Select(
							ti =>
								new
								{
									ti.Message.Name,
									ti.Message.Date,
									ti.Message.Subject,
									ti.Message.UserID,
									UserClass = (short)ti.Message.UserClass,
									ti.Message.UserNick,
									ti.Message.IsRead,
									ti.Message.IsMarked,
									ti.Message.Closed,
									ti.Message.ReadReplies,
									ti.Message.LastModerated,
									ti.Message.ArticleId,
									Rating = ti.SelfRates,
									Smiles = ti.SelfSmiles,
									Agrees = ti.SelfAgrees,
									Disagrees = ti.SelfDisagrees,
									Moderatorials = ti.SelfModeratorials,
									RepliesCount = ti.AnswersCount,
									RepliesUnread = ti.AnswersUnread,
									RepliesRate = ti.AnswersRates,
									RepliesSmiles = ti.AnswersSmiles,
									RepliesAgree = ti.AnswersAgrees,
									RepliesDisagree = ti.AnswersDisagrees,
									RepliesToMeUnread = ti.AnswersToMeUnread,
									RepliesMarked = ti.AnswersMarked,
									RepliesModeratorials = ti.AnswersModeratorials
								});
				var i = frameTop;
				foreach (var tuple in msgs)
				{
					var m = (Msg)children[i];

					m.Name = tuple.Name;
					m.Date = tuple.Date;
					m.Subject = tuple.Subject;
					m.UserID = tuple.UserID;
					m.UserClass = tuple.UserClass;
					m.UserNick = tuple.UserNick;
					m.IsRead = tuple.IsRead;
					m.IsMarked = tuple.IsMarked;
					m.Closed = tuple.Closed;
					m.ReadReplies = tuple.ReadReplies;
					m.LastModerated = tuple.LastModerated;
					m.ArticleId = tuple.ArticleId;
					m.Rating = tuple.Rating;
					m.Smiles = tuple.Smiles;
					m.Agrees = tuple.Agrees;
					m.Disagrees = tuple.Disagrees;
					m.Moderatorials = tuple.Moderatorials;
					m.RepliesCount = tuple.RepliesCount;
					m.RepliesUnread = tuple.RepliesUnread;
					m.RepliesRate = tuple.RepliesRate;
					m.RepliesSmiles = tuple.RepliesSmiles;
					m.RepliesAgree = tuple.RepliesAgree;
					m.RepliesDisagree = tuple.RepliesDisagree;
					m.RepliesToMeUnread = tuple.RepliesToMeUnread;
					m.RepliesMarked = tuple.RepliesMarked;
					m.RepliesModeratorials = tuple.RepliesModeratorials;

					m._partiallyLoaded = false;
					m.EndMapping();
					i++;
				}
			}

		}
		#endregion

		#region Original topics loading
		public static Msg GetTopics(
			IServiceProvider provider,
			int forumID,
			SortType sort,
			bool isLoadAll)
		{
			var msg = new Msg(provider);
			msg.Children = GetOldStyleTopicList(provider, forumID, sort, isLoadAll, msg);
			return msg;
		}

		private static IQueryable<ITopicInfo> AppendSortPredicate(
			IQueryable<ITopicInfo> source,
			SortType sort)
		{
			switch (sort)
			{
				case SortType.ByLastUpdateDateDesc:
					return source.OrderByDescending(ti => ti.LastUpdateDate);
				case SortType.ByLastUpdateDateAsc:
					return source.OrderBy(ti => ti.LastUpdateDate ?? ti.Message.Date);
				case SortType.ByDateAsc:
					return source.OrderBy(ti => ti.Message.Date);
				case SortType.ByDateDesc:
					return source.OrderByDescending(ti => ti.Message.Date);
				case SortType.ByIdAsc:
					return source.OrderBy(ti => ti.MessageID);
				case SortType.ByIdDesc:
					return source.OrderByDescending(ti => ti.MessageID);
				case SortType.BySubjectAsc:
					return source.OrderBy(ti => ti.Message.Subject);
				case SortType.BySubjectDesc:
					return source.OrderByDescending(ti => ti.Message.Subject);
				case SortType.ByAuthorAsc:
					return source.OrderBy(ti => ti.Message.UserNick);
				case SortType.ByAuthorDesc:
					return source.OrderByDescending(ti => ti.Message.UserNick);
				//case SortType.ByForumAsc:
				//case SortType.ByForumDesc:
				default:
					throw new ArgumentOutOfRangeException("sort");
			}
		}

		private static List<MsgBase> GetOldStyleTopicList(
			IServiceProvider provider,
			int forumId,
			SortType sort,
			bool loadAll,
			MsgBase parent)
		{
			using (var db = provider.CreateDBContext())
			{
				var displayConfig = Config.Instance.ForumDisplayConfig;
				var q =
					AppendSortPredicate(db.TopicInfos(ti => ti.ForumID == forumId), sort);
				if (displayConfig.ShowUnreadThreadsOnly)
					q = q.Where(ti => !ti.Message.IsRead || ti.AnswersUnread > 0);
				if (!(loadAll || displayConfig.MaxTopicsPerForum <= 0))
					q = q.Take(displayConfig.MaxTopicsPerForum);

				var msgs =
					q
						.Select(
							ti =>
								new Msg(provider)
								{
									Parent = parent,
									IsChild = false,

									ID = ti.MessageID,
									ParentID = 0,
									ForumID = ti.ForumID,
									Name = ti.Message.Name,
									Date = ti.Message.Date,
									Subject = ti.Message.Subject,
									UserID = ti.Message.UserID,
									UserClass = (short)ti.Message.UserClass,
									UserNick = ti.Message.UserNick,
									IsRead = ti.Message.IsRead,
									IsMarked = ti.Message.IsMarked,
									Closed = ti.Message.Closed,
									ReadReplies = ti.Message.ReadReplies,
									LastModerated = ti.Message.LastModerated,
									ArticleId = ti.Message.ArticleId,
									Rating = ti.SelfRates,
									Smiles = ti.SelfSmiles,
									Agrees = ti.SelfAgrees,
									Disagrees = ti.SelfDisagrees,
									Moderatorials = ti.SelfModeratorials,
									RepliesCount = ti.AnswersCount,
									RepliesUnread = ti.AnswersUnread,
									RepliesRate = ti.AnswersRates,
									RepliesSmiles = ti.AnswersSmiles,
									RepliesAgree = ti.AnswersAgrees,
									RepliesDisagree = ti.AnswersDisagrees,
									RepliesToMeUnread = ti.AnswersToMeUnread,
									RepliesMarked = ti.AnswersMarked,
									RepliesModeratorials = ti.AnswersModeratorials
								})
						.Cast<MsgBase>()
						.ToList();
				foreach (var msg in msgs)
					msg.EndMapping();
				return msgs;
			}
		}
		#endregion

	}
}