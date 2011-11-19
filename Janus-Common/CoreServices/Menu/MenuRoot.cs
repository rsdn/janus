using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class MenuRoot : IMenuRoot, IEquatable<MenuRoot>
	{
		private readonly string _name;
		private readonly ReadOnlyCollection<IMenuGroup> _groups;

		public MenuRoot(
			[NotNull] string name,
			[NotNull] IEnumerable<IMenuGroup> groups)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (!MenuNamesValidator.IsValidMenuName(name))
				throw new ArgumentException(@"Агрумент имеет некорректный формат.", "name");
			if (groups == null)
				throw new ArgumentNullException("groups");
			
			_name = name;
			_groups = groups.ToArray().AsReadOnly();
		}

		public string Name
		{
			get { return _name; }
		}

		public ICollection<IMenuGroup> Groups
		{
			get { return _groups; }
		}

		public bool Equals(MenuRoot obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			return Equals(obj.Name, Name)
				&& obj.Groups.SequenceEqual(Groups);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MenuRoot);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_name.GetHashCode() * 397) ^ _groups.GetHashCode();
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}