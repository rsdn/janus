using System;

namespace Janus.Model {
	public interface IDispatcher {
		bool IsInvokeRequired();
		void Invoke(Action actionToInvoke);
		void BeginInvoke(Action actionToInvoke);
	}
}