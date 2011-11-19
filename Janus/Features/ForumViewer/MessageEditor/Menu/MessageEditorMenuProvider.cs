using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class MessageEditorMenuProvider : ResourceMenuProvider
	{
		public MessageEditorMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.MessageEditor.Menu.MessageEditorMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}