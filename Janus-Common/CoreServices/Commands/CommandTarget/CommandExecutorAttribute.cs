using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[MeansImplicitUse]
	public class CommandExecutorAttribute : CommandMethodAttribute
	{
		public CommandExecutorAttribute(string commandName)
			: base(commandName) { }
	}
}