using Janus.Model.Gui;
using System;
using System.Windows;
using System.Windows.Input;

namespace Janus.Wpf.Controls.Commands {
	public class ModelCommandAdapter : DependencyObject, ICommand {
		public static DependencyProperty ModelCommandProperty = DependencyProperty.Register(nameof(ModelCommand), typeof(ModelCommand),
			typeof(ModelCommandAdapter));

		public ModelCommandAdapter() { }

		public ModelCommandAdapter(ModelCommand modelCommand) {
			ModelCommand = modelCommand;
		}

		public ModelCommand ModelCommand {
			get { return GetValue(ModelCommandProperty) as ModelCommand; }
			set {
				SetValue(ModelCommandProperty, value);
				OnCanExecuteChanged();
			}
		}

		private void OnCanExecuteChanged() {
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		public bool CanExecute(object parameter) {
			return ModelCommand?.CanExecute(parameter) ?? false;
		}

		public void Execute(object parameter) {
			ModelCommand?.Execute(parameter);
		}

		public event EventHandler CanExecuteChanged;
	}
}
