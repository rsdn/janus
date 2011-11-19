using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class ConsoleToolbarProvider : ResourceMenuProvider
	{
		public ConsoleToolbarProvider(IServiceProvider serviceProvider)
			: base(serviceProvider, "Rsdn.Janus.Core.Console.Menu.ConsoleToolbar.xml") { }
	}
}