namespace Rsdn.Janus.Framework.Networking
{
	/// <summary>
	/// Интерфейс поставщика ресурсов.
	/// </summary>
	public interface IResourceProvider
	{
		/// <summary>
		/// Имя поставщика.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Получить ресурс по указанному идентификатору ресурса.
		/// </summary>
		/// <param name="uri">идентификатор ресурса</param>
		Resource GetData(string uri);
	}
}