using System;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("moderatorials")]
	public interface IModeratorial
	{
		[Column]
		int MessageID { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Message { get; }

		[Column]
		int UserID { get; }

		[Association(ThisKey = "UserID", OtherKey = "ID", CanBeNull = true)]
		IUser User { get; }

		[Column]
		int ForumID { get; }

		[Association(ThisKey = "ForumID", OtherKey = "ID", CanBeNull = true)]
		IServerForum ServerForum { get; }

		[Column]
		DateTime Create { get; }
	}
}