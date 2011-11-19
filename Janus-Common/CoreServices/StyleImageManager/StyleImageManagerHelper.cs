using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Расширения для <see cref="IStyleImageManager"/>.
	/// </summary>
	public static class StyleImageManagerHelper
	{
		/// <summary>
		/// Добавить картинку с указанным типом в <see cref="ImageList"/>.
		/// </summary>
		/// <returns>Индекс добавлененой картинки или -1 если картинка не найдена.</returns>
		public static int AppendImage(
			this IStyleImageManager styleImageManager,
			[Localizable(false)]string name,
			StyleImageType imageType,
			ImageList imageList)
		{
			if (styleImageManager == null)
				throw new ArgumentNullException("styleImageManager");

			var img = styleImageManager.GetImage(name, imageType);
			if (img == null)
				return -1;
			imageList.Images.Add(img);
			return imageList.Images.Count - 1;
		}

		/// <summary>
		/// Попытаться получить картику, в случае если картинка найдена не будет
		/// вернуть специальную картинку, показывающую, что искомая картинка не найдена.
		/// </summary>
		public static Image TryGetImage(
			[NotNull] this IStyleImageManager styleImageManager,
			[NotNull] string name,
			StyleImageType imageType)
		{
			if (styleImageManager == null)
				throw new ArgumentNullException("styleImageManager");
			if (name == null)
				throw new ArgumentNullException("name");

			return styleImageManager.GetImage(name, imageType);
			//ToDo: сппециальная картинка для отсутствующих картинок
		}
	}
}