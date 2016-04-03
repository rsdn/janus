using System;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	public interface ICommandHandlerService
	{
		/// <summary>
		/// Выполнить команду.
		/// </summary>
		void ExecuteCommand(string commandName, ICommandContext context);

		/// <summary>
		/// Получить статус команды.
		/// </summary>
		CommandStatus QueryStatus(string commandName, ICommandContext context);

		/// <summary>
		/// Подписаться на оповещения о смене статуса команд.
		/// </summary>
		/// <param name="serviceProvider">Поставщик сервисов.</param>
		/// <param name="handler">Обработчик, вызываемый при смене статуса команд.</param>
		IDisposable SubscribeCommandStatusChanged(
			IServiceProvider serviceProvider,
			EventHandler<ICommandHandlerService, string[]> handler);
	}
}