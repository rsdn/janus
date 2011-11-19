using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class MainFormToolbarProvider : ResourceMenuProvider
	{
		public MainFormToolbarProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.UI.Menu.MainFormToolbar.xml",
				"Rsdn.Janus.SR") { }
	}
}