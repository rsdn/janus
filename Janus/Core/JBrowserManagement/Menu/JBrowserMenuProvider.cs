using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class JBrowserMenuProvider : ResourceMenuProvider
	{
		public JBrowserMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.JBrowserManagement.Menu.JBrowserMenu.xml",
				"Rsdn.Janus.Core.JBrowserManagement.Menu.JBrowserMenuResources") { }
	}
}