using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Расширение элемента дерева навигации.
	/// </summary>
	public interface INavigationTreeNodeExtension : INavigationTreeNodeSource
	{
		/// <summary>
		/// Дополнительные вложенные элементы дерева навигации.
		/// </summary>
		IList<INavigationTreeNodeSource> Childrens { get; }
	}
}