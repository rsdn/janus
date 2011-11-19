using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class SynchronizationMenuProvider : ResourceMenuProvider
	{
		public SynchronizationMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.Synchronization.UI.Menu.SynchronizationMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}