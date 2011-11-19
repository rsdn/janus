using System;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Помечает форматтер статистики.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (IStatisticsFormatter))]
	public class StatisticsFormatterAttribute : NamedElementAttribute
	{
		public StatisticsFormatterAttribute(string name) : base(name)
		{}
	}
}
