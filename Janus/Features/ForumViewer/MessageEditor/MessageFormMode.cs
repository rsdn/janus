using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Режимы работы формы сообщения
	/// </summary>
	public enum MessageFormMode
	{
		/// <summary>
		/// Создание нового сообщения
		/// </summary>
		Add = 1,
		/// <summary>
		/// Ответ на сообщение
		/// </summary>
		Reply,
		/// <summary>
		/// Редактирование сообщения
		/// </summary>
		Edit
	}
}