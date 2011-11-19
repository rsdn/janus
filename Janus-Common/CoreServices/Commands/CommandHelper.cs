using System;
using System.Collections.Generic;
using System.ComponentModel;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public static class CommandHelper
	{
		/// <summary>
		/// Временно.
		/// </summary>
		public static void TryExecuteCommand(
			this IServiceProvider provider,
			[Localizable(false)] string commandName,
			IDictionary<string, object> commandParameters)
		{
			var commandSvc = provider.GetService<ICommandHandlerService>();
			if (commandSvc != null
					&& commandSvc.QueryStatus(
						commandName,
						new CommandContext(provider, commandParameters)) == CommandStatus.Normal)
				commandSvc.ExecuteCommand(
					commandName,
					new CommandContext(provider, commandParameters));
		}

		public static void ExecuteDefaultCommand([NotNull] this IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			var defaultCommandSvc = provider.GetRequiredService<IDefaultCommandService>();
			if (defaultCommandSvc.CommandName == null)
				return;

			var commandSvc = provider.GetRequiredService<ICommandHandlerService>();
			var context = new CommandContext(provider, defaultCommandSvc.Parameters);
			if (commandSvc.QueryStatus(defaultCommandSvc.CommandName, context) == CommandStatus.Normal)
				commandSvc.ExecuteCommand(defaultCommandSvc.CommandName, context);
		}
	}
}