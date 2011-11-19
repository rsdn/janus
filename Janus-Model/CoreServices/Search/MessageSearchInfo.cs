using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Message info for full text search.
	/// </summary>
	public class MessageSearchInfo
	{
		private readonly int _messageID;
		private readonly DateTime _messageDate;
		private readonly string _subject;
		private readonly string _messageBody;
		private readonly int _forumID;
		private readonly int _userID;
		private readonly string _userNick;

		public MessageSearchInfo(
			int messageID,
			DateTime messageDate,
			string subject,
			string messageBody,
			int forumID,
			int userID,
			string userNick)
		{
			_messageID = messageID;
			_messageDate = messageDate;
			_subject = subject;
			_messageBody = messageBody;
			_forumID = forumID;
			_userID = userID;
			_userNick = userNick;
		}

		public int MessageID
		{
			get { return _messageID; }
		}

		public DateTime MessageDate
		{
			get { return _messageDate; }
		}

		public string Subject
		{
			get { return _subject; }
		}

		public string MessageBody
		{
			get { return _messageBody; }
		}

		public int ForumID
		{
			get { return _forumID; }
		}

		public int UserID
		{
			get { return _userID; }
		}

		public string UserNick
		{
			get { return _userNick; }
		}
	}
}
