using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	[Service(typeof (IBrowserConfigService))]
	internal class BrowserConfigService : IBrowserConfigService
	{
		public UrlBehavior Behavior => Config.Instance.Behavior;
	}
}