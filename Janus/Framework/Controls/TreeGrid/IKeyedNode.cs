namespace Rsdn.TreeGrid
{
	/// <summary>
	/// Интерфейс, предоставляющий уникальный ключ ноды.
	/// </summary>
	public interface IKeyedNode
	{
		/// <summary>
		/// Уникальный в пределах одного уровня иерархии ключ.
		/// </summary>
		string Key { get; }
	}
}