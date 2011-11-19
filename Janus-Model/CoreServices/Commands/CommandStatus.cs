namespace Rsdn.Janus
{
	/// <summary>
	/// Статус команды.
	/// </summary>
	public enum CommandStatus
	{
		/// <summary>
		/// Команда доступна и может быть выполнена.
		/// </summary>
		Normal,

		/// <summary>
		/// Команда недоступна, но определенные действия пользователя могут её разблокировать.
		/// </summary>
		Disabled,

		/// <summary>
		/// Команда недоступна.
		/// </summary>
		Unavailable
	}
}