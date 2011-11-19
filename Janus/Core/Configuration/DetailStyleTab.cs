using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms.PropertyGridInternal;

namespace Rsdn.Janus
{
	/// <summary>
	/// Закладка, показывающая также и расширенные настройки
	/// </summary>
	internal class DetailStyleTab : PropertiesTab
	{
		public override string TabName
		{
			get { return SR.Config.ColorSettingsDetailed; }
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
					if (stream == null)
						throw new AppDomainUnloadedException("Error loading resource");
					_bitmap = new Bitmap(stream);
				}
				return _bitmap;
			}
		}
	}
}