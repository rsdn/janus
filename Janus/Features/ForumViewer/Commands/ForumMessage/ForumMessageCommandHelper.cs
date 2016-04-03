using System;
using System.Collections.Generic;
using System.Linq;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	public static class ForumMessageCommandHelper
	{
		/// <summary>
		/// Возвращает сообщение с указанным идентификатором,
		/// либо если идентификатор не указан, возвращает текущее активное сообщение.
		/// </summary>
		public static IForumMessageInfo GetMessage(IServiceProvider serviceProvider, int? messageId)
		{
			return messageId != null
				? DatabaseManager.GetMessageWithForum(serviceProvider, messageId.Value)
				: serviceProvider
					.GetRequiredService<IActiveMessagesService>()
					.ActiveMessages
					.Single();
		}

		/// <summary>
		/// Возвращает сообщения с указанными идентификаторами, либо если 
		/// идентификаторы не указаны, возвращает текущие выделенные сообщения.
		/// </summary>
		public static IEnumerable<IForumMessageInfo> GetMessages(
			IServiceProvider serviceProvider,
			IEnumerable<int> messageIds)
		{
			return
				messageIds
					?.Select(msgId => (IForumMessageInfo)DatabaseManager.GetMessageWithForum(serviceProvider, msgId))
						?? serviceProvider
					.GetRequiredService<IActiveMessagesService>()
					.ActiveMessages;
		}

		/// <summary>
		/// Проверка актуальности установки пометки о прочтении.
		/// </summary>
		public static bool CanSetMessageReadMark(this IForumMessageInfo msg, bool isRead, bool withReplies)
		{
			// TODO: Заплата
			if (msg == null)
				return false;

			if (msg.IsUnread == isRead)
				return true;

			if (!withReplies)
				return false;

			return isRead
				? msg.RepliesUnread > 0
				: msg.RepliesUnread < msg.RepliesCount;
		}
	}
}
