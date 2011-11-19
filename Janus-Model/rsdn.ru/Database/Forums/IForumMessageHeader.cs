using System;

using BLToolkit.TypeBuilder;

namespace Rsdn.Janus
{
	[AutoImplementInterface]
	public interface IForumMessageHeader
	{
		int ID { get; }
		int ParentID { get; }
		int TopicID { get; }
		int ForumID { get; }
		string Name { get; }

		DateTime Date { get; }
		string Subject { get; }

		int UserID { get; }
		short UserClass { get; }
		string UserNick { get; }

		bool IsRead { get; }
		bool IsMarked { get; }
		bool Closed { get; }

		bool AutoRead { get; }

		DateTime? LastModerated { get; }
		int? ArticleId { get; }
	}
}