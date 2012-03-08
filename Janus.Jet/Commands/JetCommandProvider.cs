namespace Rsdn.Janus.Jet
{
	[CommandProvider]
	internal sealed class JetCommandProvider : ResourceCommandProvider
	{
		public JetCommandProvider()
			: base("Rsdn.Janus.Jet.Commands.JetCommands.xml", "Rsdn.Janus.Jet.Resources") { }
	}
}