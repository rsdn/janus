using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.ModeratorialsStats)]
	internal class ModeratorialsStatsFormatter : StatisticsFormatterBase
	{
		public ModeratorialsStatsFormatter() : base(
			() => Resources.Moderatorials1,
			() => Resources.Moderatorials2,
			() => Resources.Moderatorials5)
		{}
	}
}
