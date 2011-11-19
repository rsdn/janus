using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Rsdn.Janus.Framework.Imaging
{
	public static class Utilities
	{
		/// <summary>
		/// Собрать картинки из указанной папки и вложенных в неё.
		/// </summary>
		/// <param name="directory">начальная папка сбора</param>
		/// <param name="collectHidden">флаг сбора из скрытых папок</param>
		/// <returns>набор собранных картинок</returns>
		public static Dictionary<string, Image> CollectImages(string directory, bool collectHidden)
		{
			var images = new Dictionary<string, Image>();
			CollectNewImages(images, directory, collectHidden, string.Empty);
			return images;
		}

		/// <summary>
		/// Собрать картинки из указанной папки и вложенных в неё.
		/// </summary>
		/// <param name="directory">начальная папка сбора</param>
		/// <param name="collectHidden">флаг сбора из скрытых папок</param>
		/// <param name="namePrefix">префикс имени картинок</param>
		/// <returns>набор собранных картинок</returns>
		public static Dictionary<string, Image> CollectImages(string directory, bool collectHidden,
			string namePrefix)
		{
			var images = new Dictionary<string, Image>();
			CollectNewImages(images, directory, collectHidden, namePrefix);
			return images;
		}

		/// <summary>
		/// Собрать картинки из указанной папки и вложенных в неё, пропуская уже существующие в наборе картинки.
		/// </summary>
		/// <param name="images">набор картинок для пополнения</param>
		/// <param name="directory">начальная папка сбора</param>
		/// <param name="collectHidden">флаг сбора из скрытых папок</param>
		public static void CollectNewImages(Dictionary<string, Image> images, string directory,
			bool collectHidden)
		{
			CollectNewImages(images, directory, collectHidden, string.Empty);
		}

		private static void CollectNewImages(IDictionary<string, Image> images,
			string directory, bool collectHidden, string namePrefix)
		{
			if (!Directory.Exists(directory))
				return;

			foreach (var fileName in Directory.GetFiles(directory))
				try
				{
					var imageName = namePrefix + Path.GetFileNameWithoutExtension(fileName);
					Image img;
					if (!images.TryGetValue(imageName, out img))
					{
						img = Image.FromFile(fileName);
						images.Add(imageName, img);
					}
				}
//{{{-EmptyGeneralCatchClause
				catch
//{{{+EmptyGeneralCatchClause
				{}

			foreach (var dirName in Directory.GetDirectories(directory))
			{
				if (!collectHidden)
				{
					var info = new DirectoryInfo(dirName);
					if ((info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
						continue;
				}
				CollectNewImages(images, dirName, collectHidden,
					namePrefix + Path.GetFileName(dirName) + Path.DirectorySeparatorChar);
			}
		}
	}
}