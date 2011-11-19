using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public sealed class CommandParameterInfo
		: ICommandParameterInfo, IEquatable<CommandParameterInfo>
	{
		private readonly string _name;
		private readonly bool _isOptional;
		private readonly string _description;

		public CommandParameterInfo([NotNull] string name, bool isOptional, string description)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			_name = name;
			_description = description;
			_isOptional = isOptional;
		}

		public string Name
		{
			get { return _name; }
		}

		[CanBeNull]
		public string Description
		{
			get { return _description; }
		}

		public bool IsOptional
		{
			get { return _isOptional; }
		}

		public bool Equals(CommandParameterInfo other)
		{
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			if (other.GetType() != GetType())
				return false;
			return other._name.Equals(_name, StringComparison.OrdinalIgnoreCase)
				&& other._isOptional.Equals(_isOptional)
				&& Equals(other._description, _description);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as CommandParameterInfo);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = (_name != null ? _name.GetHashCode() : 0);
				result = (result * 397) ^ _isOptional.GetHashCode();
				result = (result * 397) ^ (_description != null ? _description.GetHashCode() : 0);
				return result;
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}