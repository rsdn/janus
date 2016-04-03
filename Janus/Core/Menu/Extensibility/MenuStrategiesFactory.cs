using System;

using CodeJam.Extensibility;
using CodeJam.Extensibility.StratFactories;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class MenuStrategiesFactory : IExtensionStrategyFactory
	{
		#region IExtensionStrategyFactory Members

		///<summary>
		/// Создает стратегии.
		///</summary>
		public IExtensionAttachmentStrategy[] CreateStrategies(
			IServiceProvider provider)
		{
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return new[] { new MenuProviderStrategy(publisher) };
		}

		#endregion
	}
}
