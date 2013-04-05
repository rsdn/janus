using System;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("moderatorials")]
	public interface IModeratorial
	{
		int MessageID { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Message { get; }

		int UserID { get; }

		[Association(ThisKey = "UserID", OtherKey = "ID", CanBeNull = true)]
		IUser User { get; }

		int ForumID { get; }

		[Association(ThisKey = "ForumID", OtherKey = "ID", CanBeNull = true)]
		IServerForum ServerForum { get; }

		DateTime Create { get; }
	}
}