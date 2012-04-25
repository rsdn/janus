namespace Rsdn.Janus
{
	/// <summary>
	/// Задача синхронизации - AT.
	/// </summary>
	public interface IWebSvcSyncTask<in T>
	{
		/// <summary>
		/// Имя задачи.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Отображаемое имя задачи.
		/// </summary>
		string GetDisplayName();

		/// <summary>
		/// Активна ли задача.
		/// </summary>
		bool IsTaskActive();

		/// <summary>
		/// Выполнить синхронизацию.
		/// </summary>
		void Sync(ISyncContext context, T svc, int retries, ITaskIndicator indicator);
	}
}