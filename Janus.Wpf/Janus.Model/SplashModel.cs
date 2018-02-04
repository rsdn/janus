using System.Reflection;

namespace Janus.Model {
	public class SplashModel : ModelBase {
		private string _Version;
		private string _Copyright;
		private string _StatusMessage;
		private bool _IsIndeterminate;
		private int _MaxProgress = 100;
		private int _CurrentProgress = 0;

		public string Version {
			get { return _Version; }
			set {
				if (string.Equals(_Version, value)) {
					return;
				}
				_Version = value;
				OnPropertyChanged();
			}
		}

		public string Copyright {
			get {
				return _Copyright;
			}
			set {
				if (string.Equals(_Copyright, value)) {
					return;
				}
				_Copyright = value;
				OnPropertyChanged();
			}
		}

		public string StatusMessage {
			get {
				return _StatusMessage;
			}
			set {
				if (string.Equals(_StatusMessage, value)) {
					return;
				}
				_StatusMessage = value;
				OnPropertyChanged();
			}
		}

		public bool IsIndeterminate {
			get { return _IsIndeterminate; }
			set {
				if (_IsIndeterminate == value) {
					return;
				}
				_IsIndeterminate = value;
				OnPropertyChanged();
			}
		}

		public int MaxProgress {
			get { return _MaxProgress; }
			set {
				if (_MaxProgress == value) {
					return;
				}
				_MaxProgress = value;
				OnPropertyChanged();
			}
		}


		public int CurrentProgress {
			get { return _CurrentProgress; }
			set {
				if (_CurrentProgress == value) {
					return;
				}
				_CurrentProgress = value;
				OnPropertyChanged();
			}
		}
		public SplashModel() {
			var product = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;
			var version = Assembly.GetEntryAssembly().GetName().Version;
			Version = $"{product} {version}";
			Copyright = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
		}
	}
}