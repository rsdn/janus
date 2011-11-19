namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class JBrowserCommandProvider : ResourceCommandProvider
	{
		public JBrowserCommandProvider()
			: base(
				"Rsdn.Janus.Core.JBrowserManagement.Commands.JBrowserCommands.xml",
				"Rsdn.Janus.Core.JBrowserManagement.Commands.JBrowserCommandResources") { }
	}
}
