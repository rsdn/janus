using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal class MessageViewerToolBarProvider : ResourceMenuProvider
	{
		public MessageViewerToolBarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Menus.MessageViewerToolbar.xml",
				"Rsdn.Janus.Properties.Resources") { }
	}
}