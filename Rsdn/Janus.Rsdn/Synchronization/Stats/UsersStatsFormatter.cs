using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	[StatisticsFormatter(JanusATInfo.UsersStats)]
	internal class UsersStatsFormatter : StatisticsFormatterBase
	{
		public UsersStatsFormatter() : base(
			() => Resources.UsersReceived1,
			() => Resources.UsersReceived2,
			() => Resources.UsersReceived5)
		{}
	}
}
