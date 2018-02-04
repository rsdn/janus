using Janus.Db.DataContext;
using Janus.Db.Forums;
using Janus.Model.Forums.Persist;
using Janus.Model.Perist;

namespace Janus.Db {
	public class JanusEFStorage : IJanusStorage {
		private JanusContext _JanusContext;
		private ForumRepository _ForumRepository;

		public JanusEFStorage(string connectionString) {
			_JanusContext = new JanusContext(connectionString);
			_ForumRepository = new ForumRepository(_JanusContext);
		}
		public IForumRepository GetForumRepository() {
			return _ForumRepository;
		}
	}
}
