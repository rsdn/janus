namespace Janus.Model.Forums {
	public class ForumTreeData : ModelBase {
		private string _Title;
		private string _ImageKey;
		private string _ImageOverlayKey;
		private bool? _IsExpanded;
		private bool _IsSelected;
		private string _Description;

		public virtual string Title {
			get { return _Title; }
			set {
				if (string.Equals(_Title, value)) {
					return;
				}
				_Title = value;
				OnPropertyChanged();
			}
		}

		public string Description {
			get { return _Description; }
			set {
				if (string.Equals(_Description, value)) {
					return;
				}
				_Description = value;
				OnPropertyChanged();
			}
		}

		public virtual string ImageKey {
			get { return _ImageKey; }
			set {
				if (string.Equals(_ImageKey, value)) {
					return;
				}
				_ImageKey = value;
				OnPropertyChanged();
			}
		}

		public virtual string ImageOverlayKey {
			get {
				return _ImageOverlayKey;
			}
			set {
				if (string.Equals(_ImageOverlayKey, value)) {
					return;
				}
				_ImageOverlayKey = value;
				OnPropertyChanged();
			}
		}

		public virtual bool? IsExpanded {
			get {
				return _IsExpanded;
			}
			set {
				if (_IsExpanded == value) {
					return;
				}
				_IsExpanded = value;
				OnPropertyChanged();
			}
		}

		public virtual bool IsSelected {
			get {
				return _IsSelected;
			}
			set {
				if (_IsSelected == value) {
					return;
				}
				_IsSelected = value;
				OnPropertyChanged();
			}
		}
	}
}