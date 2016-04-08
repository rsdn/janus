using System;

using CodeJam.Extensibility;
using CodeJam.Extensibility.StratFactories;
using CodeJam.Services;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class CommandStrategiesFactory : IExtensionStrategyFactory
	{
		public IExtensionAttachmentStrategy[] CreateStrategies(IServiceProvider provider)
		{
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return
				new[]
				{
					publisher.CreateStrategy<CommandTargetInfo, CommandTargetAttribute>(t => new CommandTargetInfo(t)),
					publisher.CreateStrategy<CommandProviderInfo, CommandProviderAttribute>(type => new CommandProviderInfo(type))
				};
		}
	}
}