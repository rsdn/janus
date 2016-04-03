using System;

using CodeJam.Extensibility;
using CodeJam.Extensibility.StratFactories;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class JanusExtStrategyFactory : IExtensionStrategyFactory
	{
		#region IExtensionStrategyFactory Members
		///<summary>
		/// Создает стратегии.
		///</summary>
		public IExtensionAttachmentStrategy[] CreateStrategies(IServiceProvider provider)
		{
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return new[]
				{
					ServicesHelper.CreateServiceStrategy(publisher),
					new ExtInfoProvidersStrategy(publisher),
					new StatsFormatterStrategy(publisher),
					new ActivePartRegStrategy(publisher)
				};
		}
		#endregion
	}
}
