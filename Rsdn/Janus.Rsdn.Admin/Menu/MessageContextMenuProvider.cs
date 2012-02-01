using System;

namespace Rsdn.Janus.Admin
{
	[MenuProvider]
	internal class MessageContextMenuProvider : ResourceMenuProvider
	{
		public MessageContextMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Admin.Menu.MessageContextMenu.xml")
		{}
	}
}
