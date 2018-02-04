using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Janus.Wpf.Controls.Converters {
	public class MarginConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			try {
				var vle = System.Convert.ToDouble(value);
				vle *= 16;
				return new Thickness(2 + vle, 2, 2, 2);
			}
			catch { return value; }
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
