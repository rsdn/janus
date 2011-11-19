using System;

namespace Rsdn.Janus
{
	public interface IStatsFormatterManager
	{
		string FormatValue(string statsName, int value, IFormatProvider formatProvider);
	}
}
