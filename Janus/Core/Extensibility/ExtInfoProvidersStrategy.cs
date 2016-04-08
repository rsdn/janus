using CodeJam.Extensibility;
using CodeJam.Extensibility.Registration;
using CodeJam.Services;

namespace Rsdn.Janus
{
	internal class ExtInfoProvidersStrategy : RegElementsStrategy<ExtensionInfoProviderInfo, ExtensionInfoProviderAttribute>
	{
		public ExtInfoProvidersStrategy(IServicePublisher publisher) : base(publisher)
		{}

		///<summary>
		/// Создать элемент.
		///</summary>
		public override ExtensionInfoProviderInfo CreateElement(
			ExtensionAttachmentContext context,
			ExtensionInfoProviderAttribute attr)
		{
			if (!typeof (IExtensionInfoProvider).IsAssignableFrom(context.Type))
				throw new ExtensibilityException(
					$"Type '{context.Type}' must implement interface '{typeof(IExtensionInfoProvider)}'");

			var infSvc = context.GetService<IBootTimeInformer>();
			if (infSvc != null)
			{
				var prov = JanusExtensionManager.CreateProvider(context, context.Type);
				infSvc.AddItem(prov.GetDisplayName(), prov.GetIcon());
			}

			return new ExtensionInfoProviderInfo(context.Type);
		}
	}
}
