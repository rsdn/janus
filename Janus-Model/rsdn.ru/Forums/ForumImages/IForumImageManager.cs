using System;
using System.Drawing;

namespace Rsdn.Janus
{
	public interface IForumImageManager
	{
		Image GetMessageImage(
			MessageType type,
			MessageFlagExistence hasUnreaded,
			bool hasUnreadRepliesToMe,
			MessageFlagExistence hasModeratorials,
			bool closed);
		Image GetMessageDateImage(DateTime date);
		Image GetUserImage(UserClass userClass);
		Image GetMarkImage(MessageFlagExistence existence);
		Image GetAutoReadImage(MessageFlagExistence existence);
		Image GetForumImage(
			bool hasUnread,
			bool inTop,
			bool hasUnreadRepliesToMe);
	}
}
