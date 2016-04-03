using System;

using CodeJam.Extensibility;

using JetBrains.Annotations;

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
