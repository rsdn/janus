using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class NavigationTreeNodeExtension : INavigationTreeNodeExtension
	{
		private readonly string _name;
		private readonly ReadOnlyCollection<INavigationTreeNodeSource> _childrens;

		public NavigationTreeNodeExtension(
			[NotNull] string name,
			[NotNull] IEnumerable<INavigationTreeNodeSource> childrens)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (childrens == null)
				throw new ArgumentNullException("childrens");

			_name = name;
			_childrens = childrens.ToArray().AsReadOnly();
		}

		#region Implementation of INavigationTreeNodeSource

		public string Name
		{
			get { return _name; }
		}

		#endregion

		#region Implementation of INavigationTreeNodeExtension

		public IList<INavigationTreeNodeSource> Childrens
		{
			get { return _childrens; }
		}

		#endregion
	}
}