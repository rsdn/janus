using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Janus.Model {
	public class ModelBase : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		public IDispatcher Dispatcher { get; set; }
		public static IDispatcher AppDispatcher { get; set; }

		protected void OnPropertyChanged([CallerMemberName]string propertyName = "") {
			var dispatcher = Dispatcher ?? AppDispatcher;
			if (dispatcher?.IsInvokeRequired() == true) {
				dispatcher.Invoke(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
			}
			else {
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}