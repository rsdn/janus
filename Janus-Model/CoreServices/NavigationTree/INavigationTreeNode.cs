using System.Collections.Generic;
using System.Windows.Forms;

using JetBrains.Annotations;

using Rsdn.TreeGrid; //временно

namespace Rsdn.Janus
{
	/// <summary>
	/// Элемент дерева навигации.
	/// </summary>
	public interface INavigationTreeNode : INavigationTreeNodeSource, IDropTarget, ITreeNode, IGetData
	{
		/// <summary>
		/// Асоциированая страница навигации.
		/// </summary>
		[NotNull]
		string NavigationPageName { get; }

		[NotNull]
		INavigationItemHeader Header { get; }

		/// <summary>
		/// Индекс для сортировки по порядку.
		/// </summary>
		int OrderIndex { get; }
		
		/// <summary>
		/// Вложенные элементы дерева навигации.
		/// </summary>
		[NotNull]
		IList<INavigationTreeNode> Childrens { get; }
	}
}