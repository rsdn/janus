using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class TrayMenuProvider : ResourceMenuProvider
	{
		public TrayMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.NotifyIcon.Menu.TrayMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}