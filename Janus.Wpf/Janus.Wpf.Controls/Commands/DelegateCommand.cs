using System;
using System.Windows.Input;

namespace Janus.Wpf.Controls.Commands {
	public class DelegateCommand : ICommand {
		public event EventHandler CanExecuteChanged;
		public Func<object, bool> DoCanExecute { get; set; }
		public Action<object> DoExecute { get; set; }

		public bool CanExecute(object parameter) {
			return DoCanExecute?.Invoke(parameter) ?? true;
		}

		public void Execute(object parameter) {
			DoExecute?.Invoke(parameter);
		}

		public void OnCanExecuteChanged() {
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
