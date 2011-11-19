namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class ConsoleCommandProvider : ResourceCommandProvider
	{
		public ConsoleCommandProvider()
			: base(
				"Rsdn.Janus.Core.Console.Commands.ConsoleCommands.xml",
				"Rsdn.Janus.Core.Console.Commands.ConsoleCommandResources") { }
	}
}