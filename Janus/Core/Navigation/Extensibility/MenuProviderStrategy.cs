using CodeJam.Extensibility;
using CodeJam.Extensibility.Registration;

namespace Rsdn.Janus
{
	internal class NavigationTreeProviderStrategy :
		RegElementsStrategy<NavigationTreeProviderInfo, NavigationTreeProviderAttribute>
	{
		public NavigationTreeProviderStrategy(IServicePublisher publisher)
			: base(publisher) { }

		public override NavigationTreeProviderInfo CreateElement(
			ExtensionAttachmentContext context,
			NavigationTreeProviderAttribute attr)
		{
			return new NavigationTreeProviderInfo(context.Type);
		}
	}
}
