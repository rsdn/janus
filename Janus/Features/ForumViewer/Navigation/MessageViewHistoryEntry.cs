namespace Rsdn.Janus
{
	/// <summary>
	/// Содержит описание точки навигации по сообщениям.
	/// </summary>
	public class MessageViewHistoryEntry
	{
		private readonly int _msgId;
		private readonly string _msgSubject;

		internal MessageViewHistoryEntry(int msgId, string msgSubject)
		{
			_msgId = msgId;
			_msgSubject = msgSubject;
		}

		/// <summary>
		/// Идентификатор сообщения.
		/// </summary>
		public int MessageId
		{
			get { return _msgId; }
		}

		/// <summary>
		/// Тема сообщения.
		/// </summary>
		public string MessageSubject
		{
			get { return _msgSubject; }
		}
	}
}