using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	using RegSvc = IRegKeyedElementsService<string, StatisticsFormatterInfo>;

	[Service(typeof (IStatsFormatterManager))]
	internal class StatsFormatterManager : IStatsFormatterManager
	{
		private readonly ElementsCache<string, IStatisticsFormatter> _formatters;

		public StatsFormatterManager(IServiceProvider provider)
		{
			_formatters = new ElementsCache<string, IStatisticsFormatter>(
				name =>
				{
					var regSvc = provider.GetService<RegSvc>();
					if (regSvc == null || !regSvc.ContainsElement(name))
						return new DefaultFormatter(name);
					return (IStatisticsFormatter)regSvc.GetElement(name).Type.CreateInstance(provider);
				});
		}

		#region IStatsFormatterManager Members
		public string FormatValue(string statsName, int value, IFormatProvider formatProvider)
		{
			return _formatters.Get(statsName).FormatValue(value, formatProvider);
		}
		#endregion

		#region DefaultFormatter class
		private class DefaultFormatter : IStatisticsFormatter
		{
			private readonly string _name;

			public DefaultFormatter(string name)
			{
				_name = name;
			}

			#region IStatisticsFormatter Members
			public string FormatValue(int value, IFormatProvider formatProvider)
			{
				return "{0} : {1}".FormatStr(_name, value.ToString(formatProvider));
			}
			#endregion
		}
		#endregion
	}
}
