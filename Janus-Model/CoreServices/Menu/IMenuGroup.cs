using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Логическая группа элементов меню.
	/// </summary>
	public interface IMenuGroup
	{
		/// <summary>
		/// Имя группы.
		/// </summary>
		[NotNull]
		string Name { get; }
		
		/// <summary>
		/// Элементы, входящие в состав группы.
		/// </summary>
		[NotNull]
		ICollection<IMenuItem> Items { get; }
		
		/// <summary>
		/// Индекс, по которому будет осуществляться сортировка.
		/// </summary>
		int OrderIndex { get; }
	}
}