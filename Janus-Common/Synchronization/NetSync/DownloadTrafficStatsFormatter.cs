namespace Rsdn.Janus
{
	[StatisticsFormatter(NetworkSyncInfo.DownloadTrafficStats)]
	public class DownloadTrafficStatsFormatter : TrafficStatsFormatter
	{
		public DownloadTrafficStatsFormatter() : base(TransferDirection.Receive)
		{}
	}
}
