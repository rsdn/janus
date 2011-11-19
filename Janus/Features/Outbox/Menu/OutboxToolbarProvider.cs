using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class OutboxToolbarProvider : ResourceMenuProvider
	{
		public OutboxToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.Outbox.Menu.OutboxToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}