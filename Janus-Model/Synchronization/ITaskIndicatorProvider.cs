namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис индикации хода задач синхронизации.
	/// </summary>
	public interface ITaskIndicatorProvider
	{
		ITaskIndicator AppendTaskIndicator(string taskName);
	}
}