namespace Rsdn.Janus.Sync
{
	internal class RsdnSyncConfig : IRsdnSyncConfig
	{
		public SyncThreadPriority SyncThreadPriority
		{
			get { return Config.Instance.SyncThreadPriority; }
		}

		public string Login
		{
			get { return Config.Instance.Login; }
		}

		public string Password
		{
			get { return LocalUser.UserPassword; }
		}

		public int SelfID
		{
			get { return Config.Instance.SelfId; }
		}

		public int MaxMessagesPerSession
		{
			get { return Config.Instance.MaxMessagesPerSession; }
		}

		public bool DownloadUsers
		{
			get { return Config.Instance.DownloadUsers; }
		}

		public int MaxUsersPerSession
		{
			get { return Config.Instance.MaxUsersPerSession; }
		}

		public bool MarkOwnMessages
		{
			get { return Config.Instance.MarkOwn; }
		}
	}
}