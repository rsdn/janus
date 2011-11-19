namespace Rsdn.Janus
{
	public enum SmartJumpBehavior
	{
		/// <summary>
		/// Переход к следующему сообщению.
		/// </summary>
		[JanusDisplayName(SR.SmartJumpToNextAnyResourceName)]
		NextAny,

		/// <summary>
		/// Переход к следующему непрочитанному сообщению.
		/// </summary>
		[JanusDisplayName(SR.SmartJumpToNextUnreadResourceName)]
		NextUnread,

		/// <summary>
		/// Переход в следующий форум.
		/// </summary>
		[JanusDisplayName(SR.SmartJumpToNextUnreadForumResourceName)]
		NextUnreadForum
	}
}