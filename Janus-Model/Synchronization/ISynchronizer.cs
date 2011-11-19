using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Синхронизатор.
	/// </summary>
	public interface ISynchronizer
	{
		/// <summary>
		/// Выполнить периодическую синхронизацию.
		/// </summary>
		IStatisticsContainer SyncPeriodic(bool activateUI);

		/// <summary>
		/// Выполнить указанную задачу синхронизации.
		/// </summary>
		IStatisticsContainer SyncSpecific(string providerName, string taskName, bool activateUI);

		/// <summary>
		/// Активна ли в данный момент синхронизация.
		/// </summary>
		bool IsActive();

		/// <summary>
		/// Доступна ли в данный момент синхронизация.
		/// </summary>
		bool IsAvailable();

		/// <summary>
		/// Оповещатель о начале синхронизации.
		/// </summary>
		IObservable<EventArgs> StartSync { get; }

		/// <summary>
		/// Оповещатель о завершении синхронизации.
		/// </summary>
		IObservable<EndSyncEventArgs> EndSync { get; }
	}
}