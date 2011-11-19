using System;

using BLToolkit.Mapping;

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

		[MapField("mid")]
		public int ID { get; set; }

		[MapField("gid")]
		public int ForumId { get; set; }

		[MapField("reply")]
		public int ReplyId { get; set; }

		[MapField("hold")]
		public bool Hold { get; set; }

		private string _subject = String.Empty;

		[MapField("subject")]
		public string Subject
		{
			get { return _subject; }
			set { _subject = value; }
		}

		private string _message = String.Empty;

		[MapField("message")]
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}
	}
}