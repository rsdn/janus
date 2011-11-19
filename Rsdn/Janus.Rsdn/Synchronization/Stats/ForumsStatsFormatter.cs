using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.ForumsStats)]
	internal class ForumsStatsFormatter : StatisticsFormatterBase
	{
		public ForumsStatsFormatter()
			: base(
				() => Resources.ForumsReceived1,
				() => Resources.ForumsReceived2,
				() => Resources.ForumsReceived5)
		{}
	}
}
