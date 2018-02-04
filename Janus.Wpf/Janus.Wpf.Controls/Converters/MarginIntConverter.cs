using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Janus.Wpf.Controls.Converters {
	public class MarginIntConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			try {
				var vle = System.Convert.ToDouble(value);
				vle *= 16;
				if ((parameter as string)?.Equals("-") ?? false) {
					vle = -vle;
				}
				return new GridLength(vle, GridUnitType.Pixel);
			}
			catch { return value; }
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
