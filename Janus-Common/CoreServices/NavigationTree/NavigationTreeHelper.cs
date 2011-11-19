using System;
using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public static class NavigationTreeHelper
	{
		public static void SelectNextNode([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			//var navigationTreeService = serviceProvider.GetRequiredService<INavigationTreeService>();
			//var navigationPageService = serviceProvider.GetRequiredService<INavigationPageService>();

			//if (navigationPageService.CurrentPage == null)
			//    return navigationTreeService.Nodes.FirstOrDefault();

			//var currentPath = navigationTreeService.GetPageNodePath(navigationPageService.CurrentPage.Name);
			//if (currentPath == null)
			//    return false;
			//if(currentPath.ParentPath==null)
			//    return GetNextNode(, )
			//navigationTreeService.CurrentPath =
			//    navigationTreeService.GetOffsetNode(navigationTreeService.CurrentPath, 1);
		}

		public static void SelectPreviousNode([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			//navigationTreeService.CurrentPath =
			//    navigationTreeService.GetOffsetNode(navigationTreeService.CurrentPath, -1);
		}

		[CanBeNull]
		private static INavigationTreeNode GetPrevNode(
			[NotNull] INavigationTreeNode parent,
			[NotNull] INavigationTreeNode current)
		{
			var nodes = parent.Childrens;
			var nodeIndex = nodes.IndexOf(current);
			return nodeIndex > 0 ? nodes[nodeIndex - 1] : parent;
		}

		[CanBeNull]
		private static Path<INavigationTreeNode> GetNextNode(
			[NotNull] Path<INavigationTreeNode> parents,
			[NotNull] INavigationTreeNode current)
		{
			var nodes = parents.LastPathComponent.Childrens;
			var nodeIndex = nodes.IndexOf(current);
			return nodeIndex < nodes.Count - 1
				? parents.Add(nodes[nodeIndex + 1])
				: parents.ParentPath != null
					? GetNextNode(parents.ParentPath, parents.LastPathComponent)
					: null;
		}
	}
}