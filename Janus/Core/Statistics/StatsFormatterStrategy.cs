using CodeJam.Extensibility;
using CodeJam.Extensibility.Registration;

namespace Rsdn.Janus
{
	internal class StatsFormatterStrategy
		: RegKeyedElementsStrategy<string, StatisticsFormatterInfo, StatisticsFormatterAttribute>
	{
		public StatsFormatterStrategy(IServicePublisher publisher) : base(publisher)
		{}

		///<summary>
		/// Создать элемент.
		///</summary>
		public override StatisticsFormatterInfo CreateElement(
			ExtensionAttachmentContext context,
			StatisticsFormatterAttribute attr)
		{
			var type = typeof (IStatisticsFormatter);
			if (!type.IsAssignableFrom(context.Type))
				throw new ExtensibilityException($"Type '{context.Type}' must implement interface '{type}'");
			return new StatisticsFormatterInfo(attr.Name, attr.Description, context.Type);
		}
	}
}
