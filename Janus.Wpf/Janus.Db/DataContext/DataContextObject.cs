using System;

namespace Janus.Db.DataContext {
	public class DataContextObject : IDisposable {
		public DataContextObject(JanusContext dataContext) {
			DataContext = dataContext;
		}

		public JanusContext DataContext { get; private set; }

		public virtual void Dispose() {

		}
	}
}