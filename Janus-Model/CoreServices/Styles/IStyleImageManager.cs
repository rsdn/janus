using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обеспечивает работу со сменяемыми картинками.
	/// </summary>
	public interface IStyleImageManager
	{
		/// <summary>
		/// Получить картинку указанного типа.
		/// </summary>
		Image GetImage(string name, StyleImageType imageType);
		
		/// <summary>
		/// Получить путь до картинки.
		/// </summary>
		string GetImageUri(string name, StyleImageType imageType);

		/// <summary>
		/// Получить картинку по заданному идентификатору.
		/// </summary>
		Image GetImage(string uri);
	}
}