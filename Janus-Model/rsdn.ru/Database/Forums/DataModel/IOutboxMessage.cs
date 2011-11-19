using System;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("messages_outbox")]
	public interface IOutboxMessage
	{
		[MapField("mid")]
		int ID { get; }

		[MapField("gid")]
		int ForumID { get; }

		[Association(ThisKey = "ForumID", OtherKey = "ID", CanBeNull = true)]
		IServerForum ServerForum { get; }

		[MapField("reply")]
		int ReplyToID { get; }

		[MapField("dte")]
		DateTime Date { get; }

		string Subject { get; }

		[MapField("message")]
		string Body { get; }

		string Tagline { get; }

		bool Hold { get; }
	}
}