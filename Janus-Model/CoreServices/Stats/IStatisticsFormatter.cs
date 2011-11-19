using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Выполняет форматированный вывод статистики.
	/// </summary>
	public interface IStatisticsFormatter
	{
		string FormatValue(int value, IFormatProvider formatProvider);
	}
}
