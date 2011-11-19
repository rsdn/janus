using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public sealed class CommandInfo : ICommandInfo
	{
		private readonly string _name;
		private readonly CommandType _type;
		private readonly Dictionary<string, ICommandParameterInfo> _parameters;
		private readonly string _displayName;
		private readonly string _description;

		public CommandInfo(
			[NotNull] string commandName,
			CommandType commandType, 
			[NotNull] IEnumerable<ICommandParameterInfo> parameters,
			string displayName,
			string description)
		{
			if (commandName == null)
				throw new ArgumentNullException("commandName");
			if (!CommandNamesValidator.IsValidCommandName(commandName))
				throw new ArgumentException(@"Имя команды имеет некорректный формат.", "commandName");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			_name = commandName;
			_type = commandType;
			_parameters = parameters.ToDictionary(
				parameter => parameter.Name,
				parameter => parameter,
				StringComparer.OrdinalIgnoreCase);
			_displayName = displayName;
			_description = description;
		}

		public string Name
		{
			get { return _name; }
		}

		public CommandType Type
		{
			get { return _type; }
		}

		public ICollection<ICommandParameterInfo> Parameters
		{
			get { return _parameters.Values; }
		}

		public ICommandParameterInfo GetParameter(string name)
		{
			return _parameters[name];
		}

		public bool IsParameterExists(string name)
		{
			return _parameters.ContainsKey(name);
		}

		[CanBeNull]
		public string DisplayName
		{
			get { return _displayName; }
		}

		[CanBeNull]
		public string Description
		{
			get { return _description; }
		}
	}
}