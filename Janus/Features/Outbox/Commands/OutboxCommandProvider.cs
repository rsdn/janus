namespace Rsdn.Janus
{
	[CommandProvider]
	internal class OutboxCommandProvider : ResourceCommandProvider
	{
		public OutboxCommandProvider()
			: base(
				"Rsdn.Janus.Features.Outbox.Commands.OutboxCommands.xml",
				"Rsdn.Janus.SR") { }
	}
}