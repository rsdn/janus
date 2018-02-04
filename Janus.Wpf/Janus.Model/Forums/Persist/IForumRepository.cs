using System.Collections.Generic;

namespace Janus.Model.Forums.Persist {
	public interface IForumRepository {
		IEnumerable<RealForumData> LoadForums(ForumRetreivalKind retreival = ForumRetreivalKind.Subscribed);
	}

	public enum ForumRetreivalKind {
		Subscribed,
		All
	}
}
