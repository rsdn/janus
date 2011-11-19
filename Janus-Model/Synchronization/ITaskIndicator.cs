namespace Rsdn.Janus
{
	/// <summary>
	/// Индикатор задачи.
	/// </summary>
	public interface ITaskIndicator
	{
		void SetTaskState(SyncTaskState state);
		void SetStatusText(string text);
	}
}