using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class LogToolbarProvider : ResourceMenuProvider
	{
		public LogToolbarProvider(IServiceProvider serviceProvider)
			: base(serviceProvider, "Rsdn.Janus.Core.LoggingManagement.Menu.LogToolbar.xml") { }
	}
}