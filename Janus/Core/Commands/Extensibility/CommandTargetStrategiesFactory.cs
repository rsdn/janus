using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class CommandTargetStrategiesFactory : IExtensionStrategyFactory
	{
		#region IExtensionStrategyFactory Members

		public IExtensionAttachmentStrategy[] CreateStrategies(
			IServiceProvider provider)
		{
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return new[] { new CommandTargetStrategy(publisher) };
		}

		#endregion
	}
}