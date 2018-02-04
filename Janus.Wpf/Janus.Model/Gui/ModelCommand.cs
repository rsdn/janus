using System;

namespace Janus.Model.Gui {
	public class ModelCommand : IModelCommand {
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
			if (ModelBase.AppDispatcher?.IsInvokeRequired() == true && CanExecuteChanged != null) {
				ModelBase.AppDispatcher.Invoke(() => CanExecuteChanged(this, EventArgs.Empty));
			}
			else {
				CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			}
		}

	}
}
