using CodeJam.Extensibility;
using CodeJam.Extensibility.Registration;

namespace Rsdn.Janus
{
	internal class MenuProviderStrategy : RegElementsStrategy<MenuProviderInfo, MenuProviderAttribute>
	{
		public MenuProviderStrategy(IServicePublisher publisher)
			: base(publisher) { }

		public override MenuProviderInfo CreateElement(
			ExtensionAttachmentContext context,
			MenuProviderAttribute attr)
		{
			return new MenuProviderInfo(context.Type);
		}
	}
}
