﻿using System;

using CodeJam.Extensibility;
using CodeJam.Extensibility.StratFactories;
using CodeJam.Services;

namespace Rsdn.Janus
{
	[ExtensionStrategyFactory]
	internal class DataStrategiesFactory : IExtensionStrategyFactory
	{
		#region IExtensionStrategyFactory Members
		///<summary>
		/// Создает стратегии.
		///</summary>
		public IExtensionAttachmentStrategy[] CreateStrategies(IServiceProvider provider)
		{
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return
				new IExtensionAttachmentStrategy[]
				{
					new DBDriverRegStrategy(publisher),
				};
		}
		#endregion
	}
}
