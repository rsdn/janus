using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class NavigationMenuProvider : ResourceMenuProvider
	{
		public NavigationMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.Navigation.Menu.NavigationMenu.xml", 
				"Rsdn.Janus.SR") { }
	}
}