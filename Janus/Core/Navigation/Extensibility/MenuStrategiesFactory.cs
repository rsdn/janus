using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class NavigationTreeStrategiesFactory : IExtensionStrategyFactory
	{
		#region IExtensionStrategyFactory Members

		///<summary>
		/// Создает стратегии.
		///</summary>
		public IExtensionAttachmentStrategy[] CreateStrategies(IServiceProvider provider)
		{
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return new[] { new NavigationTreeProviderStrategy(publisher) };
		}

		#endregion
	}
}
