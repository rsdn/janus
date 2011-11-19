namespace Rsdn.Janus
{
	public sealed class ForumSubscriptionRequest
	{
		public int ForumId { get; private set; }
		public bool IsSubscribed { get; private set; }

		public ForumSubscriptionRequest(int forumId, bool isSubscribed)
		{
			ForumId = forumId;
			IsSubscribed = isSubscribed;
		}
	}
}