using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Провайдер меню.
	/// </summary>
	public interface IMenuProvider
	{
		/// <summary>
		/// Имя меню, возвращаемого провайдером.
		/// </summary>
		[NotNull]
		string MenuName { get; }

		/// <summary>
		/// Создать меню.
		/// </summary>
		[NotNull]
		IMenuRoot CreateMenu();
	}
}