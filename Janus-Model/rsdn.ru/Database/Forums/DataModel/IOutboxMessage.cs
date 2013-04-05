using System;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("messages_outbox")]
	public interface IOutboxMessage
	{
		[Column("mid")]
		int ID { get; }

		[Column("gid")]
		int ForumID { get; }

		[Association(ThisKey = "ForumID", OtherKey = "ID", CanBeNull = true)]
		IServerForum ServerForum { get; }

		[Column("reply")]
		int ReplyToID { get; }

		[Column("dte")]
		DateTime Date { get; }

		[Column]
		string Subject { get; }

		[Column("message")]
		string Body { get; }

		[Column]
		string Tagline { get; }

		[Column]
		bool Hold { get; }
	}
}