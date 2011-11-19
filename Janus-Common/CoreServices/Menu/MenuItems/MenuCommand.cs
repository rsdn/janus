using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class MenuCommand : MenuItemWithTextAndImage, IMenuCommand, IEquatable<MenuCommand>
	{
		private readonly string _commandName;
		private readonly ReadOnlyDictionary<string, object> _parameters;

		public MenuCommand(
			[NotNull] string commandName,
			[NotNull] IDictionary<string, object> parameters,
			string text,
			string image,
			string description,
			MenuItemDisplayStyle displayStyle,
			int orderIndex)
			: base(text, image, description, displayStyle, orderIndex)
		{
			if (commandName == null)
				throw new ArgumentNullException("commandName");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			_commandName = commandName;
			_parameters = new Dictionary<string, object>(
				parameters, StringComparer.OrdinalIgnoreCase).AsReadOnly();
		}

		public string CommandName
		{
			get { return _commandName; }
		}

		public IDictionary<string, object> Parameters
		{
			get { return _parameters; }
		}

		public override void AcceptVisitor<TContext>(IMenuItemVisitor<TContext> visitor, TContext context)
		{
			visitor.Visit(this, context);
		}

	public bool Equals(MenuCommand obj)
	{
		return base.Equals(obj)
			&& obj._commandName.Equals(_commandName, StringComparison.OrdinalIgnoreCase)
			&& obj._parameters.DictionaryEquals(
				_parameters,
				StringComparer.OrdinalIgnoreCase.Equals,
				null);
	}

		public override bool Equals(object obj)
		{
			return Equals(obj as MenuCommand);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = base.GetHashCode();
				result = (result * 397) ^ _commandName.GetHashCode();
				result = (result * 397) ^ _parameters.GetHashCode();
				return result;
			}
		}
	}
}