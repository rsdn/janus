using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.OutboundMessagesStats)]
	internal class OutboudMessagesStatsFormatter : StatisticsFormatterBase
	{
		public OutboudMessagesStatsFormatter() : base(
			() => Resources.OutboundMessages1,
			() => Resources.OutboundMessages2,
			() => Resources.OutboundMessages5)
		{}
	}
}
