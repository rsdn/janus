using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class ForumMenuProvider : ResourceMenuProvider
	{
		public ForumMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Features.ForumViewer.Menu.ForumMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}