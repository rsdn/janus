using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class EndSyncEventArgs
	{
		private readonly IStatisticsContainer _statisticsContainer;
		private readonly SyncResult _result;
		private readonly Exception _exception;

		public EndSyncEventArgs(
			[NotNull] IStatisticsContainer statisticsContainer,
			SyncResult result)
			: this(statisticsContainer, result, null) {}

		public EndSyncEventArgs(
			[NotNull] IStatisticsContainer statisticsContainer,
			SyncResult result,
			Exception exception)
		{
			if (statisticsContainer == null)
				throw new ArgumentNullException("statisticsContainer");

			_statisticsContainer = statisticsContainer;
			_exception = exception;
			_result = result;
		}

		public SyncResult Result
		{
			get { return _result; }
		}

		public Exception Exception
		{
			get { return _exception; }
		}

		public IStatisticsContainer StatisticsContainer
		{
			get { return _statisticsContainer; }
		}
	}
}