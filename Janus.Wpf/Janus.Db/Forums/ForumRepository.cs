using Janus.Db.DataContext;
using Janus.Model.Forums;
using Janus.Model.Forums.Persist;
using System.Collections.Generic;
using System.Linq;

namespace Janus.Db.Forums {
	public class ForumRepository : DataContextObject, IForumRepository {
		public ForumRepository(JanusContext dataContext) : base(dataContext) {
		}

		public IEnumerable<RealForumData> LoadForums(ForumRetreivalKind retreival = ForumRetreivalKind.Subscribed) {
			var subscribedForums = DataContext.SubscribedForums;
			if (retreival != ForumRetreivalKind.Subscribed) {
				foreach (var serverForum in
					from sf in DataContext.ServerForums
					join subsF in subscribedForums on sf.id equals subsF.id
					orderby sf.descript, subsF.priority
					select new { sf, subsF }) {

					var isSubscribed = serverForum.subsF != null;
					var retForum = new RealForumData {
						Id = serverForum.sf.id,
						Title = serverForum.sf.descript,
						Description = serverForum.sf.name,
					};
					retForum.IsSubscribed = isSubscribed;
					if (isSubscribed) {
						var subsForum = serverForum.subsF;
						retForum.UnreadCount = subsForum.urcount ?? 0;
						retForum.MessageCount = DataContext.Messages.Count(msg => msg.TopicInfo.ForumId == subsForum.id);
						//retForum.AnswersToMeCount = DataContext.Messages.Where(msg => msg.)
					}
					yield return retForum;
				}
			}
			else {
				foreach (var sForum in
					from sf in DataContext.ServerForums
					join subsF in subscribedForums on sf.id equals subsF.id
					orderby sf.descript, subsF.priority
					where subsF != null
					select new { sf, subsF }) {

					var serverForum = sForum.sf;

					var retForum = new RealForumData {
						Id = serverForum.id,
						Title = serverForum.descript,
						Description = serverForum.name,
						IsSubscribed = true,
					};
					var subsForum = subscribedForums.Find(retForum.Id);
					retForum.UnreadCount = subsForum.urcount ?? 0;
					retForum.MessageCount = DataContext.Messages.Count(msg => msg.TopicInfo.ForumId == subsForum.id);

					yield return retForum;
				}
			}

		}
	}
}
