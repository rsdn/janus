using System;
using System.Collections.Generic;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	using ProviderSvc = IRegElementsService<ExtensionInfoProviderInfo>;

	/// <summary>
	/// Реализация <see cref="IJanusExtensionManager"/>
	/// </summary>
	[Service(typeof (IJanusExtensionManager))]
	internal class JanusExtensionManager : IJanusExtensionManager
	{
		private readonly List<IExtensionInfoProvider> _infoProviders =
			new List<IExtensionInfoProvider>();

		public JanusExtensionManager(IServiceProvider serviceProvider)
		{
			var svc = serviceProvider.GetService<ProviderSvc>();
			if (svc != null)
				foreach (var info in svc.GetRegisteredElements())
					_infoProviders.Add(CreateProvider(serviceProvider, info.ProviderType));
		}

		public static IExtensionInfoProvider CreateProvider(
			IServiceProvider serviceProvider,
			Type extensionType)
		{
			return (IExtensionInfoProvider)extensionType.CreateInstance(serviceProvider);
		}

		#region IJanusExtensionManager Members
		public IExtensionInfoProvider[] GetInfoProviders()
		{
			return _infoProviders.ToArray();
		}
		#endregion
	}
}
