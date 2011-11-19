using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class JBrowserToolbarProvider : ResourceMenuProvider
	{
		public JBrowserToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.JBrowserManagement.Menu.JBrowserToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}