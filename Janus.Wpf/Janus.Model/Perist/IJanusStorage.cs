using Janus.Model.Forums.Persist;

namespace Janus.Model.Perist {
	public interface IJanusStorage {
		IForumRepository GetForumRepository();
	}
}
