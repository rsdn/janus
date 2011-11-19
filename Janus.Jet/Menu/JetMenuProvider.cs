using System;

namespace Rsdn.Janus
{
	[MenuProvider]
	internal sealed class MssqlMenuProvider : ResourceMenuProvider
	{
		public MssqlMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Jet.Menu.JetMenu.xml",
				"Rsdn.Janus.Jet.Resources") { }
	}
}