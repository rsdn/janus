using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class OutboxMenuProvider : ResourceMenuProvider
	{
		public OutboxMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.Outbox.Menu.OutboxContextMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}