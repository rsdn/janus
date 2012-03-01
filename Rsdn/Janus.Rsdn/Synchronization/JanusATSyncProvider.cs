using System;
using System.Collections.Generic;

using Rsdn.Janus.AT;

namespace Rsdn.Janus
{
	/// <summary>
	/// Провайдер синхронизации для сервиса JanusAT.
	/// </summary>
	[SyncProvider(JanusATInfo.ProviderName)]
	internal class JanusATSyncProvider : WebServiceSyncProvider<JanusAT>
	{
		public JanusATSyncProvider(IServiceProvider provider)
			: base(provider, CreateSyncTasks(provider))
		{}

		protected override JanusAT CreateServiceInstance()
		{
			return new JanusATCustom(ServiceProvider, () => WebConnectionService.GetConfig());
		}

		protected override void CallSvcCheckMethod(JanusAT svc)
		{
			svc.Check();
		}

		private static IEnumerable<IWebSvcSyncTask<JanusAT>> CreateSyncTasks(
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
