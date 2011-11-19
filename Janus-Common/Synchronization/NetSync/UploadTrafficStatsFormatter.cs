namespace Rsdn.Janus
{
	[StatisticsFormatter(NetworkSyncInfo.UploadTrafficStats)]
	public class UploadTrafficStatsFormatter : TrafficStatsFormatter
	{
		public UploadTrafficStatsFormatter() : base(TransferDirection.Send)
		{}
	}
}
