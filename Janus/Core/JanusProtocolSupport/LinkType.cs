namespace Rsdn.Janus
{
	/// <summary>
	/// Тип ссылки.
	/// </summary>
	public enum LinkType
	{
		/// <summary>
		/// Локальная ссылка на сообщение.
		/// </summary>
		Local,
		/// <summary>
		/// Локальная ссылка на сообщение, отсутствующая в базе
		/// </summary>
		Absent,
		/// <summary>
		/// Внешняя ссылка
		/// </summary>
		External
	}
}