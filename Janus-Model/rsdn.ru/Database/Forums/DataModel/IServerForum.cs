using System.Collections.Generic;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("server_forums")]
	public interface IServerForum
	{
		[Column]
		int ID { get; }

		[Column]
		string Name { get; }

		[Column]
		string Descript { get; }

		[Column]
		bool Rated { get; }

		[Column]
		bool InTop { get; }

		[Column]
		int RateLimit { get; }

		[Association(ThisKey = "ID", OtherKey = "ID", CanBeNull = true)]
		ISubscribedForum SubscribedForum { get; }

		[Association(ThisKey = "ID", OtherKey = "ForumID")]
		List<ITopicInfo> TopicInfos { get; }
	}
}