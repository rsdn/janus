using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal class TextMacrosProviderStrategy :
		RegElementsStrategy<TextMacrosProviderInfo, TextMacrosProviderAttribute>
	{
		public TextMacrosProviderStrategy(IServicePublisher publisher)
			: base(publisher) { }

		public override TextMacrosProviderInfo CreateElement(
			ExtensionAttachmentContext context,
			TextMacrosProviderAttribute attr)
		{
			return new TextMacrosProviderInfo(context.Type);
		}
	}
}
