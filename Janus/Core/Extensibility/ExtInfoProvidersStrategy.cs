using Rsdn.SmartApp;

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
				throw new ExtensibilityException("Type '{0}' must implement interface '{1}'"
					.FormatStr(context.Type, typeof (IExtensionInfoProvider)));

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
