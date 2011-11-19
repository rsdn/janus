using System;

using Rsdn.Janus.AT;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal abstract class RsdnSyncTask<TRq, TRsp> : SimpleSyncTask<JanusAT, TRq, TRsp>
		where TRq : class
	{
		private readonly IServiceProvider _provider;
		private readonly IRsdnSyncConfigService _configService;

		protected RsdnSyncTask(IServiceProvider provider, string name, Func<string > displayNameGetter)
			: base(name, displayNameGetter)
		{
			_provider = provider;
			_configService = provider.GetRequiredService<IRsdnSyncConfigService>();
		}

		protected IServiceProvider Provider
		{
			get { return _provider; }
		}

		protected IRsdnSyncConfig GetSyncConfig()
		{
			return _configService.GetConfig();
		}

		protected void SetSelfID(int id)
		{
			_configService.SetSelfID(id);
		}
	}
}