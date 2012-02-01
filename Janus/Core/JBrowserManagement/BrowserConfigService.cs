using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof (IBrowserConfigService))]
	internal class BrowserConfigService : IBrowserConfigService
	{
		public UrlBehavior Behavior
		{
			get { return Config.Instance.Behavior; }
		}
	}
}