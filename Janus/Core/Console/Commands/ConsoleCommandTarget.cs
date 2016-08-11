using System;

using CodeJam;
using CodeJam.Services;

using Rsdn.Janus.Core.Console;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд окна консоли.
	/// </summary>
	[CommandTarget]
	internal sealed class ConsoleCommandTarget : CommandTarget
	{
		private const string _commandNamePrefix = "Janus.Console.";

		public ConsoleCommandTarget(IServiceProvider provider)
			: base(provider) { }

		private const string _showCommand = _commandNamePrefix + "Show";

		[CommandExecutor(_showCommand)]
		public void ExecuteConsole(ICommandContext context)
		{
			context.GetRequiredService<IConsoleWindowService>().Show();
		}

		private const string _closeCommand = _commandNamePrefix + "Close";

		[CommandExecutor(_closeCommand)]
		public void ExecuteClose(ICommandContext context)
		{
			context.GetRequiredService<IConsoleWindowService>().Close();
		}

		private const string _clearCommand = _commandNamePrefix + "Clear";

		[CommandExecutor(_clearCommand)]
		public void ExecuteClear(ICommandContext context)
		{
			context.GetRequiredService<IConsoleWindowService>().Clear();
		}

		private const string _commandInfoCommand = _commandNamePrefix + "CommandInfo";

		[CommandExecutor(_commandInfoCommand)]
		public void ExecuteCommandInfo(ICommandContext context, string command)
		{
			var commandService = context.GetRequiredService<ICommandService>();
			if (!commandService.IsCommandExists(command))
			{
				context.WriteToOutput(ConsoleResources.CommandInfoCommandNotFound);
				return;
			}

			var commandInfo = commandService.GetCommandInfo(command);

			context.WriteLineToOutput(new string('-', commandInfo.Name.Length));
			context.WriteLineToOutput(commandInfo.Name);
			context.WriteLineToOutput(new string('-', commandInfo.Name.Length));

			if (commandInfo.DisplayName.NotNullNorEmpty())
				context.WriteToOutput(commandInfo.DisplayName);

			if (commandInfo.Parameters.Count > 0)
			{
				context.WriteLineToOutput();
				context.WriteLineToOutput();
				context.WriteToOutput(ConsoleResources.CommandInfoParametersSectionTitle);
				foreach (var parameter in commandInfo.Parameters)
				{
					context.WriteLineToOutput();
					context.WriteToOutput('\t' + parameter.Name);
					if (parameter.IsOptional)
						context.WriteToOutput(" " + ConsoleResources.CommandInfoOptionalParameterMark);
					if (parameter.Description.NotNullNorEmpty())
						context.WriteToOutput(" - " + parameter.Description);
				}
			}

			if (commandInfo.Description.NotNullNorEmpty())
			{
				context.WriteLineToOutput();
				context.WriteLineToOutput();
				context.WriteLineToOutput(ConsoleResources.CommandInfoDescriptionSectionTitle);
				context.WriteToOutput(commandInfo.Description);
			}
		}

		private const string _promptCommand = _commandNamePrefix + "Prompt";

		[CommandExecutor(_promptCommand)]
		public void ExecutePrompt(ICommandContext context, string prompt)
		{
			context.GetRequiredService<IConsoleWindowService>().PromptText = prompt;
		}

		[CommandStatusGetter(_showCommand)]
		[CommandStatusGetter(_closeCommand)]
		[CommandStatusGetter(_clearCommand)]
		[CommandStatusGetter(_commandInfoCommand)]
		[CommandStatusGetter(_promptCommand)]
		public CommandStatus QueryConsoleCommandStatus(ICommandContext context)
		{
			return context.GetService<IConsoleWindowService>() != null
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}
	}
}