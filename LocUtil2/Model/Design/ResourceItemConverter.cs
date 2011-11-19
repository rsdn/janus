using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Rsdn.LocUtil.Model.Design
{
	/// <summary>
	/// Конвертер, подставляющий в качестве свойств значения.
	/// </summary>
	internal class ResourceItemConverter : TypeConverter
	{
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context,
			object value, System.Attribute[] attributes)
		{
			ArrayList props = new ArrayList(TypeDescriptor.GetProperties(value, attributes));
			IEditContext ec = (IEditContext)context.GetService(typeof (IEditContext));
			foreach (CultureInfo ci in ec.AvailableCultures)
				props.Add(new LocalePropertyDescriptor(ci));
			return new PropertyDescriptorCollection((PropertyDescriptor[])props.ToArray(
				typeof (PropertyDescriptor)));
		}

		private class LocalePropertyDescriptor : SimplePropertyDescriptor
		{
			private static readonly ResourceEditor _resourceEditor = new ResourceEditor();

			private CultureInfo _locale;

			public LocalePropertyDescriptor(CultureInfo locale)
				: base(typeof (ResourceItem), locale.ThreeLetterWindowsLanguageName, typeof (string))
			{
				_locale = locale;
			}

			public override string DisplayName
			{
				get 
				{
					if (_locale == CultureInfo.InvariantCulture)
						return "(Default)";
					else
						return _locale.DisplayName;
				}
			}

			public override object GetValue(object component)
			{
				return ((ResourceItem)component).ValueCollection[_locale];
			}

			public override void SetValue(object component, object value)
			{
				((ResourceItem)component).ValueCollection[_locale] = (string)value;
			}

			public override object GetEditor(Type editorBaseType)
			{
				return _resourceEditor;
			}
		}
	}
}
