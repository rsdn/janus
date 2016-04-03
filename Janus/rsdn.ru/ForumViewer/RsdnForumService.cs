using System;
using System.Collections.Generic;

using CodeJam.Extensibility;

using LinqToDB;

namespace Rsdn.Janus
{
	[Service(typeof (IRsdnForumService))]
	internal class RsdnForumService : IRsdnForumService
	{
		private static readonly RsdnForumConfig _config = new RsdnForumConfig();

		public IRsdnForumConfig GetConfig()
		{
			return _config;
		}

		public bool IsForumSubscribed(int forumId)
		{
			return Forums.Instance.IsSubscribed(Forums.Instance[forumId]);
		}

		public void UpdateForumAggregates(IServiceProvider provider, IDataContext db, IEnumerable<int> tidList)
		{
			DatabaseManager.UpdateAggregates(provider, db, tidList);
		}
	}
}