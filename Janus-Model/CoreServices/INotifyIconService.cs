using System.Collections.Generic;
using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис иконки в области уведомлений.
	/// </summary>
	public interface INotifyIconService
	{
		/// <summary>
		/// Показать всплыввающее уведомление.
		/// </summary>
		void ShowBalloonTip(string tipTitle, string tipText, NotificationType notificationType, int timeout);
		
		/// <summary>
		/// Текст подсказки.
		/// </summary>
		string Tooltip { get; set; }

		/// <summary>
		/// Иконка.
		/// </summary>
		Icon Icon { get; set; }
		
		/// <summary>
		/// Активность иконки.
		/// </summary>
		bool Enabled { get; set; }

		/// <summary>
		/// Видимость иконки.
		/// </summary>
		bool Visible { get; set; }

		/// <summary>
		/// Идентификатор контекстного меню.
		/// </summary>
		string ContextMenuName { get; set; }

		/// <summary>
		/// Установить дефолтную команду.
		/// </summary>
		void SetDefaultCommand(string commandName, IDictionary<string, object> parameters);
	}
}