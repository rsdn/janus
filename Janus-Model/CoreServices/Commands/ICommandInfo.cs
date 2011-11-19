using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация о команде.
	/// </summary>
	public interface ICommandInfo
	{
		/// <summary>
		/// Имя команды.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Тип команды.
		/// </summary>
		CommandType Type { get; }

		/// <summary>
		/// Информация о параметрах команды.
		/// </summary>
		ICollection<ICommandParameterInfo> Parameters { get; }

		/// <summary>
		/// Получить информацию о параметре по его имени.
		/// </summary>
		ICommandParameterInfo GetParameter(string name);

		/// <summary>
		/// Проверяет наличие у команды параметра с заданным именем.
		/// </summary>
		bool IsParameterExists(string name);

		/// <summary>
		/// Отображаемое пользователю имя команды.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Отображаемое пользователю описание команды.
		/// </summary>
		string Description { get; }
	}
}