using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class SynchronizationTrayMenuProvider : ResourceMenuProvider
	{
		public SynchronizationTrayMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.Synchronization.UI.Menu.SynchronizationTrayMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}