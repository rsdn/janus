using Janus.Model;
using System;
using System.Threading;
using System.Windows.Threading;

namespace Janus.Wpf.Controls {
	public class WpfWindowDispatcher : IDispatcher {
		private ReaderWriterLockSlim _DispatcherLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		private WpfDispatcher _RealDispatcher;
		private DispatcherObject _Window;

		public WpfWindowDispatcher() {
		}

		public WpfWindowDispatcher(DispatcherObject window) {
			Window = window;
		}

		public void BeginInvoke(Action actionToInvoke) {
			_DispatcherLock.EnterReadLock();
			try {
				_RealDispatcher.BeginInvoke(actionToInvoke);
			}
			finally {
				_DispatcherLock.ExitReadLock();
			}
		}

		public void Invoke(Action actionToInvoke) {
			_DispatcherLock.EnterReadLock();
			try {
				_RealDispatcher.Invoke(actionToInvoke);
			}
			finally {
				_DispatcherLock.ExitReadLock();
			}
		}

		public bool IsInvokeRequired() {
			_DispatcherLock.EnterReadLock();
			try {
				return _RealDispatcher.IsInvokeRequired();
			}
			finally {
				_DispatcherLock.ExitReadLock();
			}
		}

		public DispatcherObject Window {
			get { return _Window; }
			set {
				if (_Window == value) {
					return;
				}
				_Window = value;
				_DispatcherLock.EnterWriteLock();
				try {
					_RealDispatcher = new WpfDispatcher(_Window.Dispatcher);
				}
				finally {
					_DispatcherLock.ExitWriteLock();
				}
			}
		}

	}
}
