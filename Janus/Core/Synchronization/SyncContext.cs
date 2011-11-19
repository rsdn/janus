using System;

namespace Rsdn.Janus
{
	internal class SyncContext : ISyncContext
	{
		private readonly IStatisticsContainer _statsContainer;
		private readonly IServiceProvider _provider;
		private readonly Func<bool> _isSyncCancelled;

		public SyncContext(
			IServiceProvider provider,
			IStatisticsContainer statsContainer,
			Func<bool> isSyncCancelled)
		{
			if (statsContainer == null)
				throw new ArgumentNullException("statsContainer");

			_provider = provider;
			_statsContainer = statsContainer;
			_isSyncCancelled = isSyncCancelled;
		}

		#region ISyncContext Members
		public void CheckState()
		{
			if (_isSyncCancelled != null && _isSyncCancelled())
				throw new UserCancelledException();
		}

		/// <summary>
		/// Контейнер со значениями статистики.
		/// </summary>
		public IStatisticsContainer StatisticsContainer
		{
			get { return _statsContainer; }
		}
		#endregion

		#region IServiceProvider Members
		///<summary>
		/// Возвращает сервис, реализующий интерфейс T
		///</summary>
		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(ISyncContext))
				return this;
			return _provider.GetService(serviceType);
		}
		#endregion
	}
}