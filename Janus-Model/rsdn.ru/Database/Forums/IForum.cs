namespace Rsdn.Janus
{
	public interface IForum
	{
		int ID { get; }
		string Name { get; }
		int LastSync { get; }
		bool IsSubscribed { get; }
		bool IsRateable { get; }
		bool RateLimit { get; }
		int MessagesCount { get; }
		int RepliesToMeUnread { get; }
		int Unread { get; }
		bool InTop { get; }
		int Priority { get; }
		string ForumDescription { get; }
		string DisplayName { get; }
		string Description { get; }
	}
}