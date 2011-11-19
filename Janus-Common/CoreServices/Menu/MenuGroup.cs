using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public sealed class MenuGroup : IMenuGroup, IEquatable<MenuGroup>
	{
		private readonly string _name;
		private readonly ReadOnlyCollection<IMenuItem> _items;
		private readonly int _orderIndex;

		public MenuGroup(
			[NotNull] string name, 
			[NotNull] IEnumerable<IMenuItem> items,
			int orderIndex)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (!MenuNamesValidator.IsValidMenuName(name))
				throw new ArgumentException(@"Аргумент имеет некорректный формат.", "name");
			if (items == null)
				throw new ArgumentNullException("items");

			_name = name;
			_items = items.ToArray().AsReadOnly();
			_orderIndex = orderIndex;
		}

		public string Name
		{
			get { return _name; }
		}

		public ICollection<IMenuItem> Items
		{
			get { return _items; }
		}

		public int OrderIndex
		{
			get { return _orderIndex; }
		}

		public override string ToString()
		{
			return Name;
		}

		public bool Equals(MenuGroup obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != GetType())
				return false;
			return obj._name.Equals(_name, StringComparison.Ordinal)
				&& obj._items.SequenceEqual(_items)
				&& obj._orderIndex == _orderIndex;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MenuGroup);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = _name.GetHashCode();
				result = (result * 397) ^ _items.GetHashCode();
				result = (result * 397) ^ _orderIndex;
				return result;
			}
		}
	}
}
