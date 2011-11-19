using System;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис, предоставляющий дефолтную команду.
	/// </summary>
	public interface IDefaultCommandService
	{
		/// <summary>
		/// Имя дефолтной команды.
		/// </summary>
		string CommandName { get; }

		/// <summary>
		/// Параметры дефолтной команды.
		/// </summary>
		IDictionary<string, object> Parameters { get; }

		/// <summary>
		/// Изменить дефолтную команду.
		/// </summary>
		void SetDefaultCommand(string commandName, IDictionary<string, object> parameters);

		/// <summary>
		/// Событие об изменении дефолтной команды.
		/// </summary>
		IObservable<EventArgs> DefaultCommandChanged { get; }
	}
}