namespace Rsdn.Janus
{
	public interface IForumMessageInfo : IForumMessageHeader
	{
		string Body { get; }

		/// <summary>Есть ли помеченные ответы.</summary>
		bool Marked { get; }

		/// <summary>Есть ли нечитанные ответы на данное сообщение.</summary>
		bool HasRepliesUnread { get; }

		/// <summary>
		/// Прочитано ли это сообщение
		/// </summary>
		bool IsUnread { get; }

		/// <summary>Возвращает корневое сообщение (тему) или null если такая
		/// функциональность не поддерживается текущей реализацией.</summary>
		IForumMessageInfo Topic { get; }

		/// <summary>Количество ответов.</summary>
		int RepliesCount { get; }

		/// <summary>Количество помеченных (очками) в дочерних сообщениях.</summary>
		int RepliesMarked { get; }

		/// <summary>Количество ответов текущему пользователю.</summary>
		int RepliesToMeUnread { get; }

		/// <summary>Количество непрочитанных ответов.</summary>
		int RepliesUnread { get; }

		/// <summary>Дочернее сообщение или null если данная реализация
		/// не поддерживает данного свойства.</summary>
		IForumMessageInfo Parent { get; }

		string GetFormattedRating();

		/// <summary>
		/// Автоматически помечать ответы как прочитанные
		/// </summary>
		bool ReadReplies { get; }
	}
}