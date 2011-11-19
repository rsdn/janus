namespace Rsdn.Janus
{
	public class CommandStatusSubscriberAttribute : CommandMethodAttribute
	{
		public CommandStatusSubscriberAttribute(string commandName)
			: base(commandName) { }
	}
}