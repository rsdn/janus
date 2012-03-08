using System;

namespace Rsdn.Janus.Jet
{
	[MenuProvider]
	internal sealed class JetMenuProvider : ResourceMenuProvider
	{
		public JetMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Jet.Menu.JetMenu.xml",
				"Rsdn.Janus.Jet.Resources") { }
	}
}