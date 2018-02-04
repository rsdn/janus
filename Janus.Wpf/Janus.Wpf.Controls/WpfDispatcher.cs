using Janus.Model;
using System;
using System.Threading;
using System.Windows.Threading;

namespace Janus.Wpf.Controls {
	public class WpfDispatcher : IDispatcher {
		private readonly Dispatcher _Dispatcher;

		public WpfDispatcher(Dispatcher dispatcher) {
			_Dispatcher = dispatcher;
		}

		public void BeginInvoke(Action actionToInvoke) {
			_Dispatcher.BeginInvoke(actionToInvoke);
		}

		public void Invoke(Action actionToInvoke) {
			_Dispatcher.Invoke(actionToInvoke);
		}

		public bool IsInvokeRequired() {
			return _Dispatcher.Thread == Thread.CurrentThread;
		}
	}
}
