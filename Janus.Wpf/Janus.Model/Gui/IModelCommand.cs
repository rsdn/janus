using System;

namespace Janus.Model.Gui {
	public interface IModelCommand {
		event EventHandler CanExecuteChanged;
		bool CanExecute(object parameter);
		void Execute(object parameter);
		void OnCanExecuteChanged();
	}
}