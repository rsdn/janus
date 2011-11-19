using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.IndexedMessagesStats)]
	internal class IndexedMessagesStatsFormatter : StatisticsFormatterBase
	{
		public IndexedMessagesStatsFormatter() : base(
			() => Resources.Messages1 + " " + Resources.Indexed,
			() => Resources.Messages2 + " " + Resources.Indexed,
			() => Resources.Messages5 + " " + Resources.Indexed)
		{}
	}
}
