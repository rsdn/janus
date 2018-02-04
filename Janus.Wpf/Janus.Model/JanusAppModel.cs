
using Janus.Model.Perist;

namespace Janus.Model {
	public class JanusAppModel : ModelBase {
		private readonly SplashModel _Splash = new SplashModel();
		private readonly IJanusStorage _Storage;
		private MainWindowModel _MainWindow;

		public JanusAppModel(IJanusStorage storage) {
			_Storage = storage;
		}

		public IJanusStorage Storage => _Storage;
		public SplashModel Splash => _Splash;

		public MainWindowModel MainWindow {
			get {
				return _MainWindow;
			}
			set {
				_MainWindow = value;
				OnPropertyChanged();
			}
		}
	}
}
