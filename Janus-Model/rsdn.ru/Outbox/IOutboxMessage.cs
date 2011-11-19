namespace Rsdn.Janus
{
	public interface IOutboxMessage
	{
		int ID { get; }
		int ForumId { get; }
		int ReplyId { get; }
		bool Hold { get; }
		string Subject { get; }
		string Message { get; }
	}
}