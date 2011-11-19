using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class SynchronizationToolbarProvider : ResourceMenuProvider
	{
		public SynchronizationToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.Synchronization.UI.Menu.SynchronizationToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}