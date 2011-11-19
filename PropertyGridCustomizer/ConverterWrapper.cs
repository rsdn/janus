using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обертка для конвертера, поддерживающего GetProperties.
	/// </summary>
	internal class ConverterWrapper : TypeConverter
	{
		private readonly TypeConverter _wrappedConverter;

		internal ConverterWrapper(TypeConverter conv)
		{
			_wrappedConverter = conv;
		}

		public override PropertyDescriptorCollection GetProperties(
			ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			var pdc = new ArrayList(_wrappedConverter.GetProperties(context,
				value, attributes));
			pdc.Sort(new PropertyGridSortComparer());
			var pds = new PropertyDescriptor[pdc.Count];
			var i = 0;
			foreach (PropertyDescriptor pd in pdc)
			{
				pds[i] = new PropertyDescriptorWrapper(pd);
				i++;
			}
			return new PropertyDescriptorCollection(pds);
		}

		#region passing all standard methods through
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return _wrappedConverter.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return _wrappedConverter.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,
			object value)
		{
			return _wrappedConverter.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
			Type destinationType)
		{
			return _wrappedConverter.ConvertTo(context, culture, value, destinationType);
		}

		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return _wrappedConverter.CreateInstance(context, propertyValues);
		}

		public override bool Equals(object obj)
		{
			return _wrappedConverter.Equals(obj);
		}

		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return _wrappedConverter.GetCreateInstanceSupported(context);
		}

		public override int GetHashCode()
		{
			return _wrappedConverter.GetHashCode();
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return _wrappedConverter.GetPropertiesSupported(context);
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return _wrappedConverter.GetStandardValues(context);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return _wrappedConverter.GetStandardValuesExclusive(context);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return _wrappedConverter.GetStandardValuesSupported(context);
		}

		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			return _wrappedConverter.IsValid(context, value);
		}

		public override string ToString()
		{
			return _wrappedConverter.ToString();
		}
		#endregion
	}
}