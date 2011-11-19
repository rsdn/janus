using System;
using System.Linq;

using Rsdn.Janus.AT;
using Rsdn.Janus.Properties;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal class TopicSyncTask : RsdnSyncTask<TopicRequest, TopicResponse>
	{
#pragma warning disable 0649
		[ExpectService]
		private readonly IOutboxManager _outboxManager;
#pragma warning restore 0649

		public TopicSyncTask(IServiceProvider provider, string name)
			: base(provider, name, () => Resources.DownloadTopics)
		{
			this.AssignServices(provider);
		}

		public override bool IsTaskActive()
		{
			return _outboxManager.DownloadTopics.Count > 0;
		}

		protected override TopicRequest PrepareRequest(ISyncContext context)
		{
			var cfg = GetSyncConfig();
			return
				new TopicRequest
				{
					userName = cfg.Login,
					password = cfg.Password,
					messageIds =
						_outboxManager
							.DownloadTopics
							.Select(topic => topic.MessageID)
							.ToArray()
				};
		}

		protected override TopicResponse MakeRequest(ISyncContext context, JanusAT svc, TopicRequest rq)
		{
			return svc.GetTopicByMessage(rq);
		}

		protected override void ProcessResponse(ISyncContext context, TopicRequest request, TopicResponse response)
		{
			MessagesSyncHelper.AddNewMessages(
				context,
				response.Messages,
				response.Rating,
				response.Moderate,
				_outboxManager.DownloadTopics.Clear,
				GetSyncConfig().SelfID);
		}
	}
}
