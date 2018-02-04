using Janus.Model.Gui;
using Janus.Wpf.Controls.Commands;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Janus.Wpf.Controls.Converters {
	public class ModelCommandsConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is ModelCommand modelCommand) {
				return new ModelCommandAdapter {
					ModelCommand = modelCommand,
				};
			}
			else {
				return value;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
