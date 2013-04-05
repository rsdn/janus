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
			: this(0, forumId, replyId, subject, msg)
		{}

		public MessageInfo(int id, int forumId, int replyId, string subject, string msg)
		{
			ID = id;
			ForumId = forumId;
			ReplyId = replyId;
			_subject = subject;
			_message = msg;
		}

		[Column("mid")]
		public int ID { get; set; }

		[Column("gid")]
		public int ForumId { get; set; }

		[Column("reply")]
		public int ReplyId { get; set; }

		[Column("hold")]
		public bool Hold { get; set; }

		private string _subject = String.Empty;

		[Column("subject")]
		public string Subject
		{
			get { return _subject; }
			set { _subject = value; }
		}

		private string _message = String.Empty;

		[Column("message")]
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}
	}
}