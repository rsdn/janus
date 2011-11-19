using System;
using System.Globalization;
using System.Text;
using System.Linq;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public static class StatisticsHelper
	{
		public static string GetFormattedValues(
			this IStatisticsContainer container,
			IServiceProvider provider)
		{
			var statsMgr = provider.GetRequiredService<IStatsFormatterManager>();
			var sb = new StringBuilder();
			foreach (var name in container.GetStatsNames().OrderBy(nm => nm))
			{
				if (sb.Length > 0)
					sb.Append(", ");
				sb.Append(statsMgr.FormatValue(
					name,
					container.GetTotalValue(name),
					CultureInfo.CurrentUICulture));
			}
			return sb.ToString();
		}

		public static bool IsEmpty(this IStatisticsContainer container)
		{
			return !container.GetStatsNames().Any();
		}
	}
}
