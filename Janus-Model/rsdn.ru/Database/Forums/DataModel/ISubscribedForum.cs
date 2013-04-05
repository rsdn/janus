using System.Collections.Generic;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("subscribed_forums")]
	public interface ISubscribedForum
	{
		[Column]
		int ID { get; }

		[Column]
		string Name { get; }

		[Column]
		string Descript { get; }

		[Column]
		int LastSync { get; }

		[Column]
		int UrCount { get; }

		[Column]
		bool IsSync { get; }

		[Column]
		int? Priority { get; }

		[Association(ThisKey = "ID", OtherKey = "ID", CanBeNull = true)]
		IServerForum ServerForum { get; }

		[Association(ThisKey = "ID", OtherKey = "ForumID", CanBeNull = true)]
		List<ITopicInfo> TopicInfos { get; }
	}
}