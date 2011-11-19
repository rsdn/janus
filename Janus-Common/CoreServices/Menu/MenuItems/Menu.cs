using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class Menu : MenuItemWithTextAndImage, IMenu, IEquatable<Menu>
	{
		private readonly string _name;
		private readonly ReadOnlyCollection<IMenuGroup> _groups;

		public Menu(
			[NotNull] string name,
			[NotNull] IEnumerable<IMenuGroup> groups,
			string text,
			string image,
			string description,
			MenuItemDisplayStyle displayStyle,
			int orderIndex)
			: base(text, image, description, displayStyle, orderIndex)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (!MenuNamesValidator.IsValidMenuName(name))
				throw new ArgumentException(@"Аргумент имеет некорректный формат.", "name");
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

		public override void AcceptVisitor<TContext>(IMenuItemVisitor<TContext> visitor, TContext context)
		{
			visitor.Visit(this, context);
		}

		public bool Equals(Menu obj)
		{
			return base.Equals(obj)
				&& obj._name.Equals(_name, StringComparison.Ordinal)
				&& obj._groups.SequenceEqual(_groups);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Menu);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = base.GetHashCode();
				result = (result * 397) ^ _name.GetHashCode();
				result = (result * 397) ^ _groups.GetHashCode();
				return result;
			}
		}
	}
}