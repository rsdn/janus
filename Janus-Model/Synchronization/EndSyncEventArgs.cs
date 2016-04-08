using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class EndSyncEventArgs
	{
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
				throw new ArgumentNullException(nameof(statisticsContainer));

			StatisticsContainer = statisticsContainer;
			Exception = exception;
			Result = result;
		}

		public SyncResult Result { get; }

		public Exception Exception { get; }

		public IStatisticsContainer StatisticsContainer { get; }
	}
}