using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[MeansImplicitUse]
	public abstract class CommandMethodAttribute : Attribute
	{
		private readonly string _commandName;

		protected CommandMethodAttribute([NotNull] string commandName)
		{
			if (commandName == null)
				throw new ArgumentNullException("commandName");
			if (!CommandNamesValidator.IsValidCommandName(commandName))
				throw new ArgumentException("Имя команды имеет некорректный формат.", "commandName");

			_commandName = commandName;
		}

		public string CommandName
		{
			get { return _commandName; }
		}
	}
}