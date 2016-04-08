using System;

using CodeJam.Services;

using Rsdn.Janus.AT;

namespace Rsdn.Janus
{
	internal abstract class RsdnSyncTask<TRq, TRsp> : SimpleSyncTask<JanusAT, TRq, TRsp>
		where TRq : class
	{
		private readonly IRsdnSyncConfigService _configService;

		protected RsdnSyncTask(IServiceProvider provider, string name, Func<string > displayNameGetter)
			: base(name, displayNameGetter)
		{
			Provider = provider;
			_configService = provider.GetRequiredService<IRsdnSyncConfigService>();
		}

		protected IServiceProvider Provider { get; }

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