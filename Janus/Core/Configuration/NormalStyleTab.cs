using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms.PropertyGridInternal;

namespace Rsdn.Janus
{
	/// <summary>
	/// Закладка, показывающая все стандартные настройки
	/// </summary>
	internal class NormalStyleTab : PropertiesTab
	{
		public override string TabName
		{
			get { return SR.Config.ColorSettings; }
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attrs)
		{
			return FilterProperties(base.GetProperties(context, component, attrs));
		}

		public override PropertyDescriptorCollection GetProperties(object component)
		{
			return FilterProperties(base.GetProperties(component));
		}

		public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attrs)
		{
			return FilterProperties(base.GetProperties(component, attrs));
		}

		private static PropertyDescriptorCollection FilterProperties(PropertyDescriptorCollection props)
		{
			var ret = new PropertyDescriptorCollection(null);
			foreach (PropertyDescriptor desc in props)
			{
				if (desc.Attributes.Contains(DetailedAttribute.DetailedInstance) == false)
					ret.Add(desc);
			}
			return ret;
		}

		private Bitmap _bitmap;
		public override Bitmap Bitmap
		{
			get
			{
				if (_bitmap == null)
				{
					var stream = Assembly.GetExecutingAssembly()
						.GetManifestResourceStream(ApplicationInfo.ResourcesNamespace + "tabIcon.png");
					if (stream != null)
						_bitmap = new Bitmap(stream);
				}
				return _bitmap;
			}
		}
	}

}