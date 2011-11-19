using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class MenuSplitButton : MenuCommand, IMenuSplitButton, IEquatable<MenuSplitButton>
	{
		private readonly string _name;
		private readonly ReadOnlyCollection<IMenuGroup> _groups;

		public MenuSplitButton(
			[NotNull] string name,
			[NotNull] IEnumerable<IMenuGroup> groups,
			string commandName,
			IDictionary<string, object> parameters,
			string text,
			string image,
			string description,
			MenuItemDisplayStyle displayStyle,
			int orderIndex)
			: base(commandName, parameters, text, image, description, displayStyle, orderIndex)
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

		public bool Equals(MenuSplitButton obj)
		{
			return base.Equals(obj)
				&& obj._name.Equals(_name, StringComparison.Ordinal)
				&& obj._groups.SequenceEqual(_groups);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MenuSplitButton);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_name.GetHashCode() * 397) ^ _groups.GetHashCode();
			}
		}
	}
}