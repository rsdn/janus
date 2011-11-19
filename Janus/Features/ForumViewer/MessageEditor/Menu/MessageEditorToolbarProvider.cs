using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class MessageEditorToolbarProvider : ResourceMenuProvider
	{
		public MessageEditorToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.MessageEditor.Menu.MessageEditorToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}