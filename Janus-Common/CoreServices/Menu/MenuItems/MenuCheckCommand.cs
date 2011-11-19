using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class MenuCheckCommand
		: MenuItemWithTextAndImage, IMenuCheckCommand, IEquatable<MenuCheckCommand>
	{
		private readonly string _checkStateName;
		private readonly string _checkCommandName;
		private readonly ReadOnlyDictionary<string, object> _checkCommandParameters;
		private readonly string _uncheckCommandName;
		private readonly ReadOnlyDictionary<string, object> _uncheckCommandParameters;

		public MenuCheckCommand(
			[NotNull] string checkStateName,
			[NotNull] string checkCommandName,
			[NotNull] IDictionary<string, object> checkCommandParameters,
			[NotNull] string uncheckCommandName,
			[NotNull] IDictionary<string, object> uncheckCommandParameters,
			string text,
			string image,
			string description,
			MenuItemDisplayStyle displayStyle,
			int orderIndex)
			: base(text, image, description, displayStyle, orderIndex)
		{
			if (string.IsNullOrEmpty(checkStateName))
				throw new ArgumentException(
					"Аргумент не должен быть null или пустой строкой.", "checkStateName");
			if (checkCommandName == null)
				throw new ArgumentNullException("checkCommandName");
			if (checkCommandParameters == null)
				throw new ArgumentNullException("checkCommandParameters");
			if (uncheckCommandName == null)
				throw new ArgumentNullException("uncheckCommandName");
			if (uncheckCommandParameters == null)
				throw new ArgumentNullException("uncheckCommandParameters");

			_checkStateName = checkStateName;
			_checkCommandName = checkCommandName;
			_checkCommandParameters = new Dictionary<string, object>(
				checkCommandParameters, StringComparer.OrdinalIgnoreCase).AsReadOnly();
			_uncheckCommandName = uncheckCommandName;
			_uncheckCommandParameters = new Dictionary<string, object>(
				uncheckCommandParameters, StringComparer.OrdinalIgnoreCase).AsReadOnly();
		}

		public string CheckStateName
		{
			get { return _checkStateName; }
		}

		public string CheckCommandName
		{
			get { return _checkCommandName; }
		}

		public IDictionary<string, object> CheckCommandParameters
		{
			get { return _checkCommandParameters; }
		}

		public string UncheckCommandName
		{
			get { return _uncheckCommandName; }
		}

		public IDictionary<string, object> UncheckCommandParameters
		{
			get { return _uncheckCommandParameters; }
		}

		public override void AcceptVisitor<TContext>(IMenuItemVisitor<TContext> visitor, TContext context)
		{
			visitor.Visit(this, context);
		}

		public bool Equals(MenuCheckCommand obj)
		{
			return base.Equals(obj)
				&& obj._checkStateName.Equals(_checkStateName, StringComparison.OrdinalIgnoreCase)
				&& obj._checkCommandName.Equals(
					_checkCommandName, StringComparison.OrdinalIgnoreCase)
				&& !obj._checkCommandParameters.DictionaryEquals(
					_checkCommandParameters, StringComparer.OrdinalIgnoreCase.Equals, null)
				&& obj._uncheckCommandName.Equals(
					_uncheckCommandName, StringComparison.OrdinalIgnoreCase)
				&& !obj._uncheckCommandParameters.DictionaryEquals(
					_uncheckCommandParameters, StringComparer.OrdinalIgnoreCase.Equals, null);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MenuCheckCommand);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = base.GetHashCode();
				result = (result * 397) ^ _checkStateName.GetHashCode();
				result = (result * 397) ^ _checkCommandName.GetHashCode();
				result = (result * 397) ^ _checkCommandParameters.GetHashCode();
				result = (result * 397) ^ _uncheckCommandName.GetHashCode();
				result = (result * 397) ^ _uncheckCommandParameters.GetHashCode();
				return result;
			}
		}
	}
}