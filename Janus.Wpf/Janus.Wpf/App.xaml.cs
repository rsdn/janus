using Janus.Db;
using Janus.Model;
using Janus.Model.Forums.Persist;
using Janus.Model.Gui;
using Janus.Wpf.Controls;
using Janus.Wpf.Controls.DialogBoxes;
using System.ComponentModel;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Janus.Wpf {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application, INotifyPropertyChanged {
		private volatile MainWindow _MainWindow;
		private volatile Splash _Splash = null;
		private volatile bool _IsSplashClosed;
		private volatile bool _CanCloseSplash;
		private static volatile JanusAppModel _AppModel;
		private JanusEFStorage _Storage;

		public static JanusAppModel AppModel {
			get { return _AppModel; }
		}

		public new MainWindow MainWindow {
			get { return base.MainWindow as MainWindow; }
			set { base.MainWindow = value; }
		}

		private void Application_Startup(object sender, StartupEventArgs e) {
			ModelBase.AppDispatcher = new WpfDispatcher(Dispatcher);
			Task.Run(() => InitializeAndStart()).ConfigureAwait(true).GetAwaiter().OnCompleted(() => {
				_MainWindow = new MainWindow();
				_MainWindow.SetBinding(FrameworkElement.DataContextProperty, new Binding("MainWindow") { Source = _AppModel });
				void mainLoaded(object s, RoutedEventArgs a)
				{
					if (!_IsSplashClosed && _Splash != null) {
						_CanCloseSplash = true;
						_Splash.Dispatcher.Invoke(() => _Splash?.Close());
					}
					_Splash = null;
					_IsSplashClosed = true;
					_MainWindow.Activate();
					_MainWindow.BringIntoView();
					_MainWindow.Focus();
					_MainWindow.Loaded -= mainLoaded;
				};

				_MainWindow.Loaded += mainLoaded;
				MainWindow = _MainWindow;
				_MainWindow.Show();
			});
			var thr = new Thread(() => {
				_Splash = new Splash();
				while (_AppModel == null) {
					Thread.Sleep(10);
				}
				AppModel.Splash.Dispatcher = new WpfWindowDispatcher(_Splash);
				_Splash.Closing += (s, a) => {
					a.Cancel = !_CanCloseSplash;
				};
				_Splash.Closed += (s, a) => {
					_IsSplashClosed = true;
				};
				_Splash.ShowDialog();
			});
			thr.SetApartmentState(ApartmentState.STA);
			thr.Start();
		}

		private async Task InitializeAndStart() {

			UserInteraction.ConfirmationMessage += (_, args) => {
				Dispatcher.Invoke(() => {
					ModalPane.InitializeControl(MainWindow, MainWindow.rootGrid);
					var confirmModel = new ConfirmationModel {
						Buttons = args.Buttons,
						Message = args.Message,
						Title = args.Title,
					};
					var confirmDlg = new ConfirmationDialog {
						DataContext = confirmModel,
					};
					ModalPane.ShowDialog(MainWindow, confirmDlg);
					args.Result = confirmModel.Result;
				});
			};

			_Storage = new JanusEFStorage(ConfigurationManager.ConnectionStrings["Janus.Wpf.Properties.Settings.DefConnection"].ConnectionString);

			_AppModel = new JanusAppModel(_Storage);

			AppModel.MainWindow = new MainWindowModel();
			AppModel.Splash.IsIndeterminate = true;

			while (_Splash == null) {
				await Task.Delay(10);
			}

			await Task.Delay(100);
			AppModel.Splash.MaxProgress = 5;
			AppModel.Splash.CurrentProgress = 0;
			AppModel.Splash.CurrentProgress = 1;
			AppModel.Splash.IsIndeterminate = false;
			AppModel.Splash.StatusMessage = "Loading forum list…";
			var forumRepos = _Storage.GetForumRepository();

			AppModel.MainWindow.Forums.Inbox.AddForums(forumRepos.LoadForums(ForumRetreivalKind.Subscribed));
		}


		public static JanusAppModel DataModel {
			get { return AppModel; }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
