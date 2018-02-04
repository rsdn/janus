using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Janus.Model.Menu {
	public class MenuModelBase : ModelBase, IEnumerable<MenuModelBase> {
		private string _Header;
		private string _IconKey;

		public virtual IEnumerable<MenuModelBase> SubItems { get => Enumerable.Empty<MenuModelBase>(); }

		public IEnumerator<MenuModelBase> GetEnumerator() {
			foreach (var item in SubItems) {
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public string Header {
			get {
				return _Header;
			}
			set {
				if (string.Equals(_Header, value)) {
					return;
				}
				_Header = value;
				OnPropertyChanged();
			}
		}

		public string IconKey {
			get { return _IconKey; }
			set {
				if (string.Equals(_IconKey, value)) {
					return;
				}
				_IconKey = value;
			}
		}
	}
}
