namespace Rsdn.Janus
{
	/// <summary>
	/// Реализация <see cref="ITaskIndicator"/>, не выполняющая никаких действий.
	/// </summary>
	public class EmptyTaskIndicator : ITaskIndicator
	{
		#region ITaskIndicator Members
		public void SetTaskState(SyncTaskState state)
		{}

		public void SetStatusText(string text)
		{}
		#endregion
	}
}