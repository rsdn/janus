namespace Rsdn.Janus
{
	/// <summary>
	/// Интерфейс сервиса, предоставляющего возможность сохранения
	/// данных между сеансами.
	/// </summary>
	public interface IPersistentDataManager
	{
		/// <summary>
		/// Доступ к данным.
		/// </summary>
		object this[string name]
		{get; set;}
	}
}
