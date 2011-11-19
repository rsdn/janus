using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Контекст синхронизации - AT.
	/// </summary>
	public interface ISyncContext : IServiceProvider
	{
		/// <summary>
		/// Вызывает исключение, если затребовано прерывание синхронизации.
		/// </summary>
		void CheckState();

		/// <summary>
		/// Контейнер со значениями статистики.
		/// </summary>
		IStatisticsContainer StatisticsContainer { get; }
	}
}