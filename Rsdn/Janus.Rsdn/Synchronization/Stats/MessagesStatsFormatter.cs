using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.MessagesStats)]
	internal class MessagesStatsFormatter : StatisticsFormatterBase
	{
		public MessagesStatsFormatter() : base(
			() => Resources.InboundMessages1,
			() => Resources.InboundMessages2,
			() => Resources.InboundMessages5)
		{}
	}
}
