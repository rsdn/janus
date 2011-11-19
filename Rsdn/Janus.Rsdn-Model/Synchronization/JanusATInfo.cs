namespace Rsdn.Janus
{
	/// <summary>
	/// Константы, связанные с сервисом синхронизации JanusAT.
	/// </summary>
	public static class JanusATInfo
	{
		public const string ProviderName = "JanusATProvider";

		private const string _statsPrefix = "JanusAT:";

		/// <summary>
		/// Статистика добавленных в индекс сообщений.
		/// </summary>
		public const string IndexedMessagesStats = _statsPrefix + "indexed-messages";

		/// <summary>
		/// Статистика количества входящих сообщений.
		/// </summary>
		public const string MessagesStats = _statsPrefix + "messages";

		/// <summary>
		/// Статистика количества бомбочек.
		/// </summary>
		public const string ModeratorialsStats = _statsPrefix + "moderatorials";

		/// <summary>
		/// Статистика количества входящих оценок.
		/// </summary>
		public const string RatesStats = _statsPrefix + "rates";

		/// <summary>
		/// Статистика полученных форумов.
		/// </summary>
		public const string ForumsStats = _statsPrefix + "forums";

		/// <summary>
		/// Статистика новых форумов.
		/// </summary>
		public const string NewForumsStat = _statsPrefix + "new-forums";

		/// <summary>
		/// Статистика количества отправленных сообщений.
		/// </summary>
		public const string OutboundMessagesStats = _statsPrefix + "outbound-messages";

		/// <summary>
		/// Статистика количества неотправленных сообщений.
		/// </summary>
		public const string FailedOutboundMessagesStats = _statsPrefix + "failed-outbound-messages";

		/// <summary>
		/// Статистика количества отправленных оценок.
		/// </summary>
		public const string OutboundRatesStats = _statsPrefix + "outbound-rates";

		/// <summary>
		/// Статистика количества полученных пользователей.
		/// </summary>
		public const string UsersStats = _statsPrefix + "users";

		public const string MessagesSyncTaskName = "MessagesSync";
		public const string ForumsSyncTaskName = "ForumsSync";
		public const string TopicSyncTaskName = "TopicSync";
		public const string UsersSyncTaskName = "UsersSync";
		public const string PostMessagesSyncTaskName = "PostMessagesSync";
	}
}