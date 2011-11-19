using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("server_forums")]
	public interface IServerForum
	{
		int ID { get; }

		string Name { get; }

		string Descript { get; }

		bool Rated { get; }

		bool InTop { get; }

		int RateLimit { get; }

		[Association(ThisKey = "ID", OtherKey = "ID", CanBeNull = true)]
		ISubscribedForum SubscribedForum { get; }

		[Association(ThisKey = "ID", OtherKey = "ForumID")]
		List<ITopicInfo> TopicInfos { get; }
	}
}