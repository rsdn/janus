namespace Rsdn.Janus
{
	/// <summary>
	/// Тип команды.
	/// </summary>
	public enum CommandType
	{
		/// <summary>
		/// Обычная команда.
		/// </summary>
		Default,

		/// <summary>
		/// Команда нуждается во взаимодействии с пользователем.
		/// </summary>
		RequiresInteraction
	}
}