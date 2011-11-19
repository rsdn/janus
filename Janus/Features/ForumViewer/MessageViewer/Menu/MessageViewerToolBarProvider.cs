using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal class MessageViewerToolBarProvider : ResourceMenuProvider
	{
		public MessageViewerToolBarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.MessageViewer.Menu.MessageViewerToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}