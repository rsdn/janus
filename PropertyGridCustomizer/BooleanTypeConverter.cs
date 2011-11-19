using System;
using System.ComponentModel;
using System.Globalization;

using Rsdn.PropGridCust;

namespace Rsdn.Janus
{
	/// <summary>
	/// TypeConverter для bool
	/// </summary>
	internal class BooleanTypeConverter : BooleanConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
			object value, Type destType)
		{
			return (bool)value
				?
					SR.BooleanTrue
				: SR.BooleanFalse;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,
			object value)
		{
			return (string)value == SR.BooleanTrue;
		}
	}
}