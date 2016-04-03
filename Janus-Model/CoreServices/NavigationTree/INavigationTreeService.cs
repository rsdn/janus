using System;
using System.Collections.Generic;

using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис дерева навигации.
	/// </summary>
	public interface INavigationTreeService
	{
		/// <summary>
		/// Элементы дерева навигации.
		/// </summary>
		[NotNull]
		IList<INavigationTreeNode> Nodes { get; }
		
		/// <summary>
		/// Событие об изменении дерева навигации.
		/// </summary>
		[NotNull]
		IObservable<EventArgs> TreeChanged { get; }

		/// <summary>
		/// Возвращает путьь до элемента дерева с указанной страницей.
		/// </summary>
		[CanBeNull]
		Path<INavigationTreeNode> GetPageNodePath([NotNull] string pageName);
	}
}