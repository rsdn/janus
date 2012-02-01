//ToDo: перенести в Janus-Model, когда будет произведен переход на новый интерфейс сообщений

using System;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис, предоставляющий текущие активные сообщения.
	/// </summary>
	public interface IActiveMessagesService
	{
		/// <summary>
		/// Активные сообщения.
		/// </summary>
		IEnumerable<IForumMessageInfo> ActiveMessages { get; }

		/// <summary>
		/// Возникает когда активные сообщения изменились.
		/// </summary>
		event EventHandler ActiveMessagesChanged;
	}
}