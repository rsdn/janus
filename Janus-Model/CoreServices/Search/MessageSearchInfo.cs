using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Message info for full text search.
	/// </summary>
	public class MessageSearchInfo
	{
		public MessageSearchInfo(
			int messageID,
			DateTime messageDate,
			string subject,
			string messageBody,
			int forumID,
			int userID,
			string userNick)
		{
			MessageID = messageID;
			MessageDate = messageDate;
			Subject = subject;
			MessageBody = messageBody;
			ForumID = forumID;
			UserID = userID;
			UserNick = userNick;
		}

		public int MessageID { get; }

		public DateTime MessageDate { get; }

		public string Subject { get; }

		public string MessageBody { get; }

		public int ForumID { get; }

		public int UserID { get; }

		public string UserNick { get; }
	}
}
