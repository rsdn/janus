namespace Rsdn.Janus
{
	/// <summary>
	/// Провайдер синхронизации.
	/// </summary>
	public interface ISyncProvider
	{
		/// <summary>
		/// Проверка доступности.
		/// </summary>
		bool IsAvailable();

		/// <summary>
		/// Запустить указанную задачу.
		/// </summary>
		void SyncTask(ISyncContext context, string taskName);

		/// <summary>
		/// Запустить периодические задачи.
		/// </summary>
		void SyncPeriodicTasks(ISyncContext context);
	}
}