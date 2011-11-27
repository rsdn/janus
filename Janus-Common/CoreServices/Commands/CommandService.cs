using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(ICommandService))]
	public class CommandService : ICommandService
	{
		private readonly Dictionary<string, ICommandInfo> _commands = 
			new Dictionary<string, ICommandInfo>(StringComparer.OrdinalIgnoreCase);

		public CommandService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			foreach (var commandInfo in
				new ExtensionsCache<CommandProviderInfo, ICommandProvider>(serviceProvider)
					.GetAllExtensions()
					.SelectMany(commandProvider => commandProvider.CreateCommands()))
			{
				if (_commands.ContainsKey(commandInfo.Name))
					throw new ApplicationException(
						"Команда '{0}' определена более одного раза.".FormatStr(commandInfo.Name));
				_commands.Add(commandInfo.Name, commandInfo);
			}
		}

		#region ICommandService Members

		public ICollection<ICommandInfo> Commands
		{
			get { return _commands.Values; }
		}

		public ICommandInfo GetCommandInfo(string commandName)
		{
			if (commandName == null)
				throw new ArgumentNullException("commandName");

			ICommandInfo result;
			if (!_commands.TryGetValue(commandName, out result))
				throw new ArgumentException(
					"Команда '{0}' не найдена.".FormatStr(commandName), "commandName");

			return result;
		}

		public bool IsCommandExists(string commandName)
		{
			if (commandName == null)
				throw new ArgumentNullException("commandName");

			return _commands.ContainsKey(commandName);
		}

		#endregion
	}
}