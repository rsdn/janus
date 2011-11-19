using System;
using System.ComponentModel;
using System.Globalization;

namespace Rsdn.Janus
{
	/// <summary>
	/// TypeConverter для енумов, преобразовывающий енумы к строке с 
	/// учетом атрибута DisplayName
	/// </summary>
	internal class EnumTypeConverter : EnumConverter
	{
		private readonly Type _EnumType;

		/// <summary>
		/// Инициализирует экземпляр
		/// </summary>
		/// <param name="type">тип енума</param>
		public EnumTypeConverter(Type type) : base(type)
		{
			_EnumType = type;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
		{
			return destType == typeof (string);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
			object value, Type destType)
		{
			var fi = _EnumType.GetField(Enum.GetName(_EnumType, value));
			var dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(
				fi, typeof (DisplayNameAttribute));
			return dna != null ? dna.DisplayName : value.ToString();
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
		{
			return srcType == typeof (string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,
			object value)
		{
			foreach (var fi in _EnumType.GetFields())
			{
				var dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(
					fi, typeof (DisplayNameAttribute));
				if ((dna != null) && ((string)value == dna.DisplayName))
					return Enum.Parse(_EnumType, fi.Name);
			}
			return Enum.Parse(_EnumType, (string)value);
		}
	}
}