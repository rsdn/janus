using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.FailedOutboundMessagesStats)]
	internal class FailedOutboundMessagesStatsFormatter : StatisticsFormatterBase
	{
		public FailedOutboundMessagesStatsFormatter() : base(
			() => Resources.FailedOutboundMessages1,
			() => Resources.FailedOutboundMessages2,
			() => Resources.FailedOutboundMessages5)
		{}
	}
}
