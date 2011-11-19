using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class MssqlMenuProvider : ResourceMenuProvider
	{
		public MssqlMenuProvider(IServiceProvider serviceProvider) 
			: base(
				serviceProvider,
				"Rsdn.Janus.Mssql.Menu.MssqlMenu.xml",
				"Rsdn.Janus.Mssql.Resources") { }
	}
}