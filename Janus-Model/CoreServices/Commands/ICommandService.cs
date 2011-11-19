using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис команд.
	/// </summary>
	public interface ICommandService
	{
		/// <summary>
		/// Список описаний всех команд.
		/// </summary>
		[NotNull]
		ICollection<ICommandInfo> Commands { get; }

		/// <summary>
		/// Получить информацию о команде по её имени.
		/// </summary>
		[NotNull]
		ICommandInfo GetCommandInfo([NotNull] string commandName);

		/// <summary>
		/// Проверить наличие команды.
		/// </summary>
		bool IsCommandExists([NotNull] string commandName);
	}
}