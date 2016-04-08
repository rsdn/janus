using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

using CodeJam.Services;

namespace Rsdn.Janus
{
	public static class ForumCommandHelper
	{
		/// <summary>
		/// Дефолтная логика получения статуса команды производящей операции над
		/// единственным сообщением.
		/// </summary>
		public static CommandStatus GetSingleMessageCommandStatus(
			   IServiceProvider serviceProvider, int? messageId)
		{
			if (messageId != null)
				return CommandStatus.Normal;

			var activeMsgSvc = serviceProvider.GetService<IActiveMessagesService>();
			return activeMsgSvc != null && activeMsgSvc.ActiveMessages.Count() == 1
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		/// <summary>
		/// Дефолтная логика получения статуса команды производящей
		/// операции над одним или несколькими сообщениями форума.
		/// </summary>
		public static CommandStatus GetMultipleMessagesCommandStatus(
			IServiceProvider serviceProvider, int[] messageIds)
		{
			if (messageIds != null)
				return CommandStatus.Normal;

			var activeMsgSvc = serviceProvider.GetService<IActiveMessagesService>();

			return activeMsgSvc != null && activeMsgSvc.ActiveMessages.Any()
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		/// <summary>
		/// Дефолтная логика подписки на оповешения о смене статуса команды.
		/// Обновление статуса происходит при возниконовении события 
		/// <see cref="IActiveMessagesService.ActiveMessagesChanged"/> севиса
		/// <see cref="IActiveMessagesService"/>.
		/// </summary>
		public static IDisposable SubscribeMessageCommandStatusChanged(
			IServiceProvider serviceProvider, Action handler)
		{
			var activeMsgSvc = serviceProvider.GetService<IActiveMessagesService>();
			if (activeMsgSvc != null)
			{
				EventHandler statusUpdater = (sender, e) => handler();
				activeMsgSvc.ActiveMessagesChanged += statusUpdater;
				return Disposable.Create(
					() => activeMsgSvc.ActiveMessagesChanged -= statusUpdater);
			}
			return Disposable.Empty;
		}

		/// <summary>
		/// Возвращает указанный идентификатор сообщения,
		/// либо если идентификатор не указан, возвращает идентификатор текущего активного сообщения.
		/// </summary>
		public static int GetMessageId(IServiceProvider serviceProvider, int? messageId)
		{
			return
				messageId
					?? serviceProvider
						.GetRequiredService<IActiveMessagesService>()
						.ActiveMessages
						.Single()
						.ID;
		}

		/// <summary>
		/// Возвращает указанные идентификаторы сообщений, либо если идентификаторы
		/// не указаны, возвращает идентификаторы текущих активных сообщений.
		/// </summary>
		public static IEnumerable<int> GetMessageIds(
			IServiceProvider serviceProvider, IEnumerable<int> messageIds)
		{
			return messageIds
				?? serviceProvider
					.GetRequiredService<IActiveMessagesService>()
					.ActiveMessages
					.Select(msg => msg.ID);
		}
	}
}
