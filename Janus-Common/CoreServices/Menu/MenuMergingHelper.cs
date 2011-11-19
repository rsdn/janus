using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class MenuMergingHelper
	{
		public static IEnumerable<IMenuRoot> MergeMenuRoots([NotNull] IEnumerable<IMenuRoot> roots)
		{
			if (roots == null)
				throw new ArgumentNullException("roots");

			return 
				roots
					.GroupBy(
						root => root.Name,
						(groupName, groupItems) =>
							groupItems.IsSingle()
								? groupItems.First()
								: new MenuRoot(
									groupName,
									MergeMenuGroups(groupItems.SelectMany(menuInfo => menuInfo.Groups))),
						StringComparer.OrdinalIgnoreCase);
		}

		public static IEnumerable<IMenuGroup> MergeMenuGroups([NotNull] IEnumerable<IMenuGroup> groups)
		{
			if (groups == null)
				throw new ArgumentNullException("groups");

			return 
				groups
					.GroupBy(
						group => group.Name,
						(key, keyedGroups) =>
							keyedGroups.IsSingle()
								? keyedGroups.First()
								: new MenuGroup(
									key,
									MergeMenuItems(keyedGroups.SelectMany(group => group.Items)),
									keyedGroups.First().OrderIndex),
						StringComparer.OrdinalIgnoreCase)
					.Sort(group => group.OrderIndex);
		}

		public static IEnumerable<IMenuItem> MergeMenuItems([NotNull] IEnumerable<IMenuItem> items)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			return MergeMenuItemsInternal(items).Sort(item => item.OrderIndex);
		}

		#region Private Members

		private static IEnumerable<IMenuItem> MergeMenuItemsInternal(
			IEnumerable<IMenuItem> items)
		{
			var mergingInfos = GroupContainers(items.OfType<IContainerMenuItem>());
			foreach (var menuItem in items)
			{
				var containerMenuItem = menuItem as IContainerMenuItem;
				if (containerMenuItem != null)
				{
					var mergingInfo = mergingInfos[containerMenuItem.Name];
					if (mergingInfo.IsCrated)
						continue;
					mergingInfo.IsCrated = true;

					yield return
						mergingInfo.ContainersToMerge.Count == 1
							? mergingInfo.ContainersToMerge[0]
							: CreateMenuItem(mergingInfo.ContainerType, mergingInfo.ContainersToMerge);
				}
				else
					yield return menuItem;
			}
		}

		private static IContainerMenuItem CreateMenuItem(
			ContainerMenuItemType type,
			IList<IContainerMenuItem> items)
		{
			switch (type)
			{
				case ContainerMenuItemType.Menu:
					var firstMenu = (IMenu)items[0];
					return new Menu(
						firstMenu.Name,
						MergeMenuGroups(items.SelectMany(c => c.Groups)),
						firstMenu.Text,
						firstMenu.Image,
						firstMenu.Description,
						firstMenu.DisplayStyle,
						firstMenu.OrderIndex);
				case ContainerMenuItemType.SplitButton:
					var firstSplitButton = (IMenuSplitButton)items[0];
					return new MenuSplitButton(
						firstSplitButton.Name,
						MergeMenuGroups(items.SelectMany(c => c.Groups)),
						firstSplitButton.CommandName,
						firstSplitButton.Parameters,
						firstSplitButton.Text,
						firstSplitButton.Image,
						firstSplitButton.Description,
						firstSplitButton.DisplayStyle,
						firstSplitButton.OrderIndex);
			}
			throw new NotSupportedException();
		}

		private static ContainerMenuItemType GetContainerMenuItemType(
			IContainerMenuItem item)
		{
			if (item is IMenuSplitButton)
				return ContainerMenuItemType.SplitButton;
			if (item is IMenu)
				return ContainerMenuItemType.Menu;
			throw new NotSupportedException();
		}

		private static Dictionary<string, ContainerMergingInfo> GroupContainers(
			IEnumerable<IContainerMenuItem> containers)
		{
			var result = new Dictionary<string, ContainerMergingInfo>(StringComparer.OrdinalIgnoreCase);
			foreach (var container in containers)
			{
				var containerType = GetContainerMenuItemType(container);
				ContainerMergingInfo info;
				if (result.TryGetValue(container.Name, out info))
				{
					if (info.ContainerType != containerType)
						throw new ApplicationException();
					info.ContainersToMerge.Add(container);
				}
				else
					result.Add(
						container.Name,
						new ContainerMergingInfo(containerType, container));
			}
			return result;
		}

		#region ContainerMergingInfo
		private sealed class ContainerMergingInfo
		{
			public List<IContainerMenuItem> ContainersToMerge { get; private set; }
			public ContainerMenuItemType ContainerType { get; private set; }
			public bool IsCrated { get; set; }

			public ContainerMergingInfo(
				ContainerMenuItemType containerType,
				IContainerMenuItem initialElement)
			{
				ContainerType = containerType;
				ContainersToMerge = new List<IContainerMenuItem> { initialElement };
			}
		}
		#endregion

		#region ContainerMenuItemType
		private enum ContainerMenuItemType
		{
			Menu,
			SplitButton
		}
		#endregion

		#endregion
	}
}