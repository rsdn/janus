using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class DatabaseMenuProvider : ResourceMenuProvider
	{
		public DatabaseMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Core.DataManagement.Menu.DatabaseMenu.xml",
				"Rsdn.Janus.SR") { }
	}
}