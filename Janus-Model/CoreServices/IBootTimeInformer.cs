using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис информирования о процессе загрузки.
	/// </summary>
	public interface IBootTimeInformer
	{
		void SetText(string text);
		void AddItem(string itemText, Image itemIcon);
	}
}
