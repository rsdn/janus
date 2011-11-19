using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Провайдер команд.
	/// </summary>
	public interface ICommandProvider
	{
		/// <summary>
		/// Создает команды.
		/// </summary>
		IEnumerable<ICommandInfo> CreateCommands();
	}
}