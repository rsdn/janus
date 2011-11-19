namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class SynchronizationCommandProvider : ResourceCommandProvider
	{
		public SynchronizationCommandProvider()
			: base(
				"Rsdn.Janus.Core.Synchronization.Commands.SynchronizationCommands.xml",
				"Rsdn.Janus.SR") { }
	}
}