namespace Rsdn.Janus
{
	/// <summary>
	/// Состояние заачи синхронизации.
	/// </summary>
	public enum SyncTaskState
	{
		WaitForSync,
		Sync,
		Succeed,
		Failed
	}
}