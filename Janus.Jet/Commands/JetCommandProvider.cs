namespace Rsdn.Janus.Jet
{
	[CommandProvider]
	internal sealed class FirebirdCommandProvider : ResourceCommandProvider
	{
		public FirebirdCommandProvider()
			: base("Rsdn.Janus.Jet.Commands.JetCommands.xml", "Rsdn.Janus.Jet.Resources") { }
	}
}