using System;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис для асинхронной установки пометки о прочтении сообщений.
	/// </summary>
	public interface IMessageMarkService
	{
		void QueueMessageMark(IEnumerable<ForumEntryIds> msgIds, bool isRead, Action markFinished);
	}
}