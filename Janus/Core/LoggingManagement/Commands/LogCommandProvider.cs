namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class LogCommandProvider : ResourceCommandProvider
	{
		public LogCommandProvider()
			: base(
				"Rsdn.Janus.Core.LoggingManagement.Commands.LogCommands.xml",
				"Rsdn.Janus.Core.LoggingManagement.Commands.LogCommandResources") { }
	}
}