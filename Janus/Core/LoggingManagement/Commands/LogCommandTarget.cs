using System;

using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд окна лога.
	/// </summary>
	[CommandTarget]
	internal sealed class LogCommandTarget : CommandTarget
	{
		private const string _commandNamePrefix = "Janus.Log.";

		public LogCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		private const string _showCommand = _commandNamePrefix + "Show";

		[CommandExecutor(_showCommand)]
		public void ExecuteLog(ICommandContext context)
		{
			context.GetRequiredService<ILogWindowService>().Show();
		}

		private const string _closeCommand = _commandNamePrefix + "Close";

		[CommandExecutor(_closeCommand)]
		public void ExecuteClose(ICommandContext context)
		{
			context.GetRequiredService<ILogWindowService>().Close();
		}

		private const string _clearCommand = _commandNamePrefix + "Clear";

		[CommandExecutor(_clearCommand)]
		public void ExecuteClear(ICommandContext context)
		{
			context.GetRequiredService<ILogWindowService>().Clear();
		}

		[CommandStatusGetter(_showCommand)]
		[CommandStatusGetter(_closeCommand)]
		[CommandStatusGetter(_clearCommand)]
		public CommandStatus QueryLogCommandStatus(ICommandContext context)
		{
			return context.GetService<ILogWindowService>() != null
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}
	}
}