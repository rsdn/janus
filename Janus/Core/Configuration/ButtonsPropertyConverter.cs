using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

using Rsdn.Janus.Core.Configuration;

namespace Rsdn.Janus
{
	/// <summary>
	/// Конвертер стиля оформления. Предназначен для ограничения набора стилей.
	/// </summary>
	internal class ButtonsPropertyConverter : TypeConverter
	{
		private readonly List<string> _buttonsValuesCollection;

		public ButtonsPropertyConverter()
		{
			_buttonsValuesCollection = new List<string>(
				StyleImageManager.CollectStyleDirs());
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value.GetType() == typeof(string))
			{
				var newValue = (string)value;
				if (!_buttonsValuesCollection.Contains(newValue))
					throw new Exception(string.Format(ConfigResources.ButtonsPropertyConverter_InvalidValue, value));
				return newValue;
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(_buttonsValuesCollection);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}