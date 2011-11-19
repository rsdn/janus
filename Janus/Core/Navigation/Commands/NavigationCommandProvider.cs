namespace Rsdn.Janus
{
	[CommandProvider]
	internal sealed class NavigationCommandProvider : ResourceCommandProvider
	{
		public NavigationCommandProvider()
			: base(
				"Rsdn.Janus.Core.Navigation.Commands.NavigationCommands.xml",
				"Rsdn.Janus.SR") { }
	}
}