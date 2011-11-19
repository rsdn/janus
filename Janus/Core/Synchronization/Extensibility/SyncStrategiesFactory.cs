using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class SyncStrategiesFactory : IExtensionStrategyFactory
	{
		#region IExtensionStrategyFactory Members
		///<summary>
		/// Создает стратегии.
		///</summary>
		public IExtensionAttachmentStrategy[] CreateStrategies(IServiceProvider provider)
		{
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return
				new[]
				{
					publisher.CreateStrategy<string, SyncProviderInfo, SyncProviderAttribute>(
						(ctx, attr) => new SyncProviderInfo(attr.Name, ctx.Type, attr.Description))
				};
		}
		#endregion
	}
}
