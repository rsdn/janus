namespace Rsdn.Janus
{
	public interface IRsdnSyncConfig
	{
		SyncThreadPriority SyncThreadPriority { get; }

		string Login { get; }
		string Password { get; }
		int SelfID { get; }

		int MaxMessagesPerSession { get; }
		bool DownloadUsers { get; }
		int MaxUsersPerSession { get; }

		bool MarkOwnMessages { get; }
	}
}