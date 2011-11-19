using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.RatesStats)]
	internal class RatesStatsFormatter : StatisticsFormatterBase
	{
		public RatesStatsFormatter() : base(
			() => Resources.InboundRates1,
			() => Resources.InboundRates2,
			() => Resources.InboundRates5)
		{}
	}
}
