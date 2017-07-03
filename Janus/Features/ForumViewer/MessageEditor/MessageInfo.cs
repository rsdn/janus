using System;

using LinqToDB.Mapping;

namespace Rsdn.Janus
{
	/// <summary>
	/// DTO для сообщения
	/// </summary>
	public class MessageInfo : IOutboxMessage
	{
		public MessageInfo()
		{
		}

		public MessageInfo(int forumId)
		{
			ForumId = forumId;
		}

		public MessageInfo(int forumId, int replyId, string subject, string msg)
			: this(0, forumId, replyId, subject, msg, null)
		{}

		public MessageInfo(int id, int forumId, int replyId, string subject, string msg, string tags)
		{
			ID = id;
			ForumId = forumId;
			ReplyId = replyId;
			Subject = subject;
			Message = msg;
			Tags = tags;
		}

		[Column("mid")]
		public int ID { get; set; }

		[Column("gid")]
		public int ForumId { get; set; }

		[Column("reply")]
		public int ReplyId { get; set; }

		[Column("hold")]
		public bool Hold { get; set; }

		[Column("subject")]
		public string Subject { get; set; } = string.Empty;

		[Column("message")]
		public string Message { get; set; } = string.Empty;

		[Column("tags")]
		public string Tags { get; set; }
	}
}