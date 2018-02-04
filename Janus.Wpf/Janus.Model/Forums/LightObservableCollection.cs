using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Janus.Model.Forums {
	public class LightObservableCollection<T> : ObservableCollection<T> {
		private volatile int _UpdateLevel;
		public void BeginUpdate() {
			Interlocked.Increment(ref _UpdateLevel);
		}
		public void EndUpdate() {
			if (Interlocked.Decrement(ref _UpdateLevel) == 0) {
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
			if (_UpdateLevel <= 0) {
				if (ModelBase.AppDispatcher.IsInvokeRequired()) {
					ModelBase.AppDispatcher.Invoke(() => base.OnCollectionChanged(e));
				}
				else {
					base.OnCollectionChanged(e);
				}
			}
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
			if (_UpdateLevel <= 0) {
				if (ModelBase.AppDispatcher.IsInvokeRequired()) {
					ModelBase.AppDispatcher.Invoke(() => base.OnPropertyChanged(e));
				}
				else {
					base.OnPropertyChanged(e);
				}
			}
		}

		public void AddRange(IEnumerable<T> items) {
			BeginUpdate();
			try {
				if (items is IQueryable<T>) {
					items = items.ToArray();
				}
				foreach (var item in items) {
					Add(item);
				}
			}
			finally {
				EndUpdate();
			}
		}
	}
}