using Janus.Wpf.Properties;
using System.Windows;
using System.Windows.Data;

namespace Janus.Wpf {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			SetBinding(WindowStateProperty,
				new Binding(nameof(Settings.MainWindowState)) {
					Source = Settings.Default,
					Mode = BindingMode.TwoWay,
				});
		}
	}
}
