namespace Rsdn.Janus
{
	/// <summary>
	/// Имформация о параметре команды.
	/// </summary>
	public interface ICommandParameterInfo
	{
		/// <summary>
		/// Имя параметра.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Описание параметра.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Опциональность параметра.
		/// </summary>
		bool IsOptional { get; }
	}
}