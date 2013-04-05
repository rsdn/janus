using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("messages"), UsedImplicitly]
	public interface IForumMessage
	{
		[Identity]
		[Column("mid")]
		int ID { get; }

		[Column("tid")]
		int TopicID { get; }

		[Column("pid")]
		int ParentID { get; }

		[Association(ThisKey = "ParentID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Parent { get; }

		[Column("gid")]
		int ForumID { get; }

		[Association(ThisKey = "ForumID", OtherKey = "ID", CanBeNull = true)]
		IServerForum ServerForum { get; }

		[Column]
		string Subject { get; }

		[Column("dte")]
		DateTime Date { get; }

		[Column("uid")]
		int UserID { get; }

		[Association(ThisKey = "UserID", OtherKey = "ID", CanBeNull = true)]
		IUser User { get; }

		[Column("uclass")]
		UserClass UserClass { get; }

		[Column]
		string UserNick { get; }

		[Column]
		string Name { get; }

		[Column("article_id")]
		int? ArticleId { get; }

		[Column]
		DateTime? LastModerated { get; }

		[Column]
		bool Closed { get; }

		[Column]
		string Message { get; }

		[Column]
		bool IsMarked { get; }

		[Column]
		bool IsRead { get; }

		[Column]
		bool ReadReplies { get; }

		[Association(ThisKey = "ID", OtherKey = "MessageID")]
		IList<IRate> Rates { get; }

		[Association(ThisKey = "ID", OtherKey = "MessageID")]
		IList<IModeratorial> Moderatorials { get; }

		[Association(ThisKey = "ID", OtherKey = "TopicID")]
		IList<IForumMessage> TopicAnswers { get; }

		[Association(ThisKey = "ID", OtherKey = "MessageID", CanBeNull = true)]
		IViolation Violation { get; }
	}
}