using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация о форматтере статистики.
	/// </summary>
	public class StatisticsFormatterInfo : KeyedElementInfo<string>
	{
		public StatisticsFormatterInfo(string name, string description, Type type)
			: base(type, name, description)
		{}
	}
}
