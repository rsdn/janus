using System;
using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

namespace Rsdn.Janus.DataModel
{
	[TableName("messages"), UsedImplicitly]
	public interface IForumMessage
	{
		[Identity]
		[MapField("mid")]
		int ID { get; }

		[MapField("tid")]
		int TopicID { get; }

		[MapField("pid")]
		int ParentID { get; }

		[Association(ThisKey = "ParentID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Parent { get; }

		[MapField("gid")]
		int ForumID { get; }

		[Association(ThisKey = "ForumID", OtherKey = "ID", CanBeNull = true)]
		IServerForum ServerForum { get; }

		string Subject { get; }

		[MapField("dte")]
		DateTime Date { get; }

		[MapField("uid")]
		int UserID { get; }

		[Association(ThisKey = "UserID", OtherKey = "ID", CanBeNull = true)]
		IUser User { get; }

		[MapField("uclass")]
		UserClass UserClass { get; }

		string UserNick { get; }

		string Name { get; }

		[MapField("article_id")]
		int? ArticleId { get; }

		DateTime? LastModerated { get; }

		bool Closed { get; }

		string Message { get; }

		bool IsMarked { get; }

		bool IsRead { get; }

		bool ReadReplies { get; }

		[Association(ThisKey = "ID", OtherKey = "MessageID")]
		IList<IRate> Rates { get; }

		[Association(ThisKey = "ID", OtherKey = "MessageID")]
		IList<IModeratorial> Moderatorials { get; }

		[Association(ThisKey = "ID", OtherKey = "TopicID")]
		IList<IForumMessage> TopicAnswers { get; }
	}
}