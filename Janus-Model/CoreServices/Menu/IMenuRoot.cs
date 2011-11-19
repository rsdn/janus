using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Корень меню. Представляет собой всю строку меню или тулбар.
	/// </summary>
	public interface IMenuRoot
	{
		/// <summary>
		/// Имя меню.
		/// </summary>
		[NotNull]
		string Name { get; }

		/// <summary>
		/// Группы, входящие в состав меню.
		/// </summary>
		[NotNull]
		ICollection<IMenuGroup> Groups { get; }
	}
}