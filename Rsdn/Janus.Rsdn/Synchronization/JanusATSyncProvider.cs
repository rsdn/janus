using System;
using System.Collections.Generic;

using Rsdn.Janus.AT;

using JanusAT = Rsdn.Janus.org.rsdn.JanusAT;

namespace Rsdn.Janus
{
	/// <summary>
	/// Провайдер синхронизации для сервиса JanusAT.
	/// </summary>
	[SyncProvider(JanusATInfo.ProviderName)]
	internal class JanusATSyncProvider : WebServiceSyncProvider<org.rsdn.JanusAT>
	{
		public JanusATSyncProvider(IServiceProvider provider)
			: base(provider, CreateSyncTasks(provider))
		{}

		protected override org.rsdn.JanusAT CreateServiceInstance()
		{
			return new JanusATCustom(ServiceProvider, () => WebConnectionService.GetConfig());
		}

		protected override void CallSvcCheckMethod(org.rsdn.JanusAT svc)
		{
			svc.Check();
		}

		private static IEnumerable<IWebSvcSyncTask<org.rsdn.JanusAT>> CreateSyncTasks(
			IServiceProvider provider)
		{
			return
				new IWebSvcSyncTask<JanusAT>[]
				{
					new ForumsSyncTask(provider, JanusATInfo.ForumsSyncTaskName),
					new MessagesSyncTask(provider, JanusATInfo.MessagesSyncTaskName),
					new PostMessagesSyncTask(provider, JanusATInfo.PostMessagesSyncTaskName),
					new TopicSyncTask(provider, JanusATInfo.TopicSyncTaskName),
					new UsersSyncTask(provider, JanusATInfo.UsersSyncTaskName),
					new ViolationsSyncTask(provider, JanusATInfo.ViolationsSyncTaskName)
				};
		}

		protected override string[] GetPeriodicTaskNames()
		{
			return
				new[]
				{
					JanusATInfo.PostMessagesSyncTaskName,
					JanusATInfo.MessagesSyncTaskName,
					JanusATInfo.TopicSyncTaskName,
					JanusATInfo.UsersSyncTaskName,
					JanusATInfo.ViolationsSyncTaskName
				};
		}

		//protected override void OnSyncSessionFinished(JanusAT svc)
		//{
		//  base.OnSyncSessionFinished(svc);
		//  // Counters logic place here
		//}
	}
}
