//ToDo: перенести в Janus-Model, когда будет произведен переход на новый интерфейс сообщений

using System;
using System.Collections.Generic;

using Rsdn.Janus.ObjectModel;

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
		IEnumerable<IMsg> ActiveMessages { get; }

		/// <summary>
		/// Возникает когда активные сообщения изменились.
		/// </summary>
		event EventHandler ActiveMessagesChanged;
	}
}