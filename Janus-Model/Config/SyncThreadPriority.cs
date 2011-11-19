namespace Rsdn.Janus
{
	/// <summary>
	/// Приоритет потока синхронизации.
	/// </summary>
	public enum SyncThreadPriority
	{
		/// <summary>
		/// Нормальный.
		/// </summary>
		[ConfigDisplayName("SyncThreadPriorityLow")]
		Normal,

		/// <summary>
		/// Низкий.
		/// </summary>
		[ConfigDisplayName("SyncThreadPriorityNormal")]
		Low
	}
}