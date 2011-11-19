namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class SearchCommandProvider : ResourceCommandProvider
	{
		public SearchCommandProvider()
			: base("Rsdn.Janus.Features.Search.Commands.SearchCommands.xml", "Rsdn.Janus.SR") { }
	}
}