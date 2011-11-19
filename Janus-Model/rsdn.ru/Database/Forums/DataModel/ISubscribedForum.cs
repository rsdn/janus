using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("subscribed_forums")]
	public interface ISubscribedForum
	{
		int ID { get; }

		string Name { get; }

		string Descript { get; }

		int LastSync { get; }

		int UrCount { get; }

		bool IsSync { get; }

		int? Priority { get; }

		[Association(ThisKey = "ID", OtherKey = "ID", CanBeNull = true)]
		IServerForum ServerForum { get; }

		[Association(ThisKey = "ID", OtherKey = "ForumID", CanBeNull = true)]
		List<ITopicInfo> TopicInfos { get; }
	}
}