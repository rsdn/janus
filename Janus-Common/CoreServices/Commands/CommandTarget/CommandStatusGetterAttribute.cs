using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[MeansImplicitUse]
	public class CommandStatusGetterAttribute : CommandMethodAttribute
	{
		public CommandStatusGetterAttribute(string commandName)
			: base(commandName) { }
	}
}