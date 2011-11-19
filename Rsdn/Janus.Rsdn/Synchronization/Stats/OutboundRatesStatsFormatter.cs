using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.OutboundRatesStats)]
	internal class OutboundRatesStatsFormatter : StatisticsFormatterBase
	{
		public OutboundRatesStatsFormatter() : base(
			() => Resources.OutboundRates1,
			() => Resources.OutboundRates2,
			() => Resources.OutboundRates5)
		{}
	}
}
