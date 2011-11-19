using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class ForumEntryIdsExtensions
	{
		public static ForumEntryType GetEntryType([NotNull] this ForumEntryIds forumEntryIds)
		{
			if (forumEntryIds == null)
				throw new ArgumentNullException("forumEntryIds");

			if (forumEntryIds.MessageId != null)
				return ForumEntryType.Message;
			if (forumEntryIds.TopicId != null)
				return ForumEntryType.Topic;
			if (forumEntryIds.ForumId != null)
				return ForumEntryType.Forum;

			return ForumEntryType.AllForums;
		}

		public static bool IsContainsForum(
			[NotNull] this IEnumerable<ForumEntryIds> forumEntriesIds,
			int forumId)
		{
			bool isWholeForum;
			return IsContainsForum(forumEntriesIds, forumId, out isWholeForum);
		}

		public static bool IsContainsForum(
			[NotNull] this IEnumerable<ForumEntryIds> forumEntriesIds,
			int forumId,
			out bool isWholeForum)
		{
			if (forumEntriesIds == null)
				throw new ArgumentNullException("forumEntriesIds");

			var isContains = false;
			foreach (var ids in forumEntriesIds)
			{
				switch (ids.GetEntryType())
				{
					case ForumEntryType.AllForums:
						isWholeForum = true;
						return true;
					case ForumEntryType.Forum:
						if (ids.ForumId == forumId)
						{
							isWholeForum = true;
							return true;
						}
						break;
					default:
						if (ids.ForumId == forumId)
							isContains = true;
						break;
				}
			}
			isWholeForum = false;
			return isContains;
		}
	}

	public enum ForumEntryType
	{
		Message = 0,
		Topic = 1,
		Forum = 2,
		AllForums = 3
	}
}