using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class ForumMenuProvider : ResourceMenuProvider
	{
		public ForumMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Admin.Menu.ForumMenu.xml",
				"Rsdn.Janus.Admin.Commands.AdminCommandResources") { }
	}
}