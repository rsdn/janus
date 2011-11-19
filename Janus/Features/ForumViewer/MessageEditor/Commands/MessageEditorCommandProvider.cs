namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class MessageEditorCommandProvider : ResourceCommandProvider
	{
		public MessageEditorCommandProvider()
			: base(
				"Rsdn.Janus.Features.ForumViewer.MessageEditor.Commands.MessageEditorCommands.xml",
				"Rsdn.Janus.SR") { }
	}
}