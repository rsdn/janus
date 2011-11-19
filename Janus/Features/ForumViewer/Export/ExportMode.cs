namespace Rsdn.Janus
{
	/// <summary>
	/// Источник сообщений для экспорта.
	/// </summary>
	public enum ExportMode
	{
		/// <summary>
		/// Текущие сообщения.
		/// </summary>
		Messages,

		/// <summary>
		/// Текущие темы.
		/// </summary>
		Topics,

		/// <summary>
		/// Текущий форум.
		/// </summary>
		Forum
	}
}