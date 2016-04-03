using System;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд.
	/// </summary>
	public interface ICommandTarget
	{
		/// <summary>
		/// Выплнить команду.
		/// </summary>
		void Execute(string commandName, ICommandContext context);

		/// <summary>
		/// Получить статус команды.
		/// </summary>
		CommandStatus QueryStatus(string commandName, ICommandContext context);

		/// <summary>
		/// Подписаться на оповещения о смене статуса команд.
		/// </summary>
		/// <param name="serviceProvider">Поставщик сервисов.</param>
		/// <param name="handler">Обработчик, вызываемый при смене статуса команд.</param>
		IDisposable SubscribeStatusChanged(
			IServiceProvider serviceProvider,
			EventHandler<ICommandTarget, string[]> handler);
	}
}