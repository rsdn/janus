using Rsdn.SmartApp;

namespace Rsdn.Janus.Sync
{
	[Service(typeof (IRsdnSyncConfigService))]
	internal class RsdnSyncConfigService : IRsdnSyncConfigService
	{
		private static readonly RsdnSyncConfig _config = new RsdnSyncConfig();

		public IRsdnSyncConfig GetConfig()
		{
			return _config;
		}

		public void SetSelfID(int id)
		{
			Config.Instance.SelfId = id;
		}
	}
}