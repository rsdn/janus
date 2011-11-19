using System;
using System.ComponentModel;
using System.Globalization;

using Rsdn.PropGridCust;

namespace Rsdn.Janus
{
	/// <summary>
	/// TypeConverter для редактируемых коллекций
	/// </summary>
	internal class CollectionTypeConverter : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
		{
			return destType == typeof (string);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
			object value, Type destType)
		{
			return SR.CollectionName;
		}
	}
}