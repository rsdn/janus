using System;

namespace Rsdn.Janus
{
	public sealed class ForumEntryIds
	{
		public static readonly ForumEntryIds AllForums = new ForumEntryIds();

		private ForumEntryIds() : this(null) { }

		public ForumEntryIds(int? forumId)
			: this(forumId, null) { }

		public ForumEntryIds(int? forumId, int? topicId)
			: this(forumId, topicId, null) { }

		public ForumEntryIds(int? forumId, int? topicId, int? messageId)
		{
			if (messageId != null && topicId == null)
				throw new ArgumentNullException(nameof(topicId));

			ForumId = forumId;
			MessageId = messageId;
			TopicId = topicId;
		}

		public int? MessageId { get; }

		public int? TopicId { get; }

		public int? ForumId { get; }
	}
}