using System;

using CodeJam.Extensibility;
using CodeJam.Extensibility.StratFactories;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class CheckStateSourceStrategiesFactory : IExtensionStrategyFactory
	{
		#region IExtensionStrategyFactory Members

		public IExtensionAttachmentStrategy[] CreateStrategies(IServiceProvider provider)
		{
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return
				new[]
					{
						publisher
							.CreateStrategy<CheckStateSourceInfo, CheckStateSourceAttribute>(
								type => new CheckStateSourceInfo(type))
					};
		}

		#endregion
	}
}