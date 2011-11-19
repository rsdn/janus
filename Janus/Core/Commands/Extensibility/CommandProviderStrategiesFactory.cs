using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class CommandProviderStrategiesFactory : IExtensionStrategyFactory
	{
		public static IExtensionAttachmentStrategy CreateCommandProviderStrategy(IServiceProvider provider)
		{
			return
				provider.CreateStrategy<CommandProviderInfo, CommandProviderAttribute>(
					type => new CommandProviderInfo(type));
		}

		public IExtensionAttachmentStrategy[] CreateStrategies(
			IServiceProvider provider)
		{
			return new[] { CreateCommandProviderStrategy(provider) };
		}
	}
}