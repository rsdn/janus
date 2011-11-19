using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class MainFormMenuProvider : ResourceMenuProvider
	{
		public MainFormMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.UI.Menu.MainFormMenu.xml", 
				"Rsdn.Janus.SR") { }
	}
}