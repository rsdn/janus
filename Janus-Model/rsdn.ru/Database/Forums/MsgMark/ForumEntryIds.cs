using System;

namespace Rsdn.Janus
{
	public sealed class ForumEntryIds
	{
		private readonly int? _forumId;
		private readonly int? _topicId;
		private readonly int? _messageId;

		public static readonly ForumEntryIds AllForums = new ForumEntryIds();

		private ForumEntryIds() : this(null) { }

		public ForumEntryIds(int? forumId)
			: this(forumId, null) { }

		public ForumEntryIds(int? forumId, int? topicId)
			: this(forumId, topicId, null) { }

		public ForumEntryIds(int? forumId, int? topicId, int? messageId)
		{
			if (messageId != null && topicId == null)
				throw new ArgumentNullException("topicId");

			_forumId = forumId;
			_messageId = messageId;
			_topicId = topicId;
		}

		public int? MessageId
		{
			get { return _messageId; }
		}

		public int? TopicId
		{
			get { return _topicId; }
		}

		public int? ForumId
		{
			get { return _forumId; }
		}
	}
}