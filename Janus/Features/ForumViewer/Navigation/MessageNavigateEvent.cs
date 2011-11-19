using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Аргументы события MessageNavigate
	/// </summary>
	public class MessageNavigateEventArgs : EventArgs
	{
		private readonly int _MsgId;
		private readonly HistoryNavigationType _NavType;

		internal MessageNavigateEventArgs(int msgId, HistoryNavigationType navType)
		{
			_MsgId = msgId;
			_NavType = navType;
		}

		/// <summary>
		/// Идентификатор сообщения
		/// </summary>
		public int MessageId
		{
			get { return _MsgId; }
		}

		/// <summary>
		/// Тема сообщения
		/// </summary>
		public string MessageSubject { get; set; }

		/// <summary>
		/// Тип навигации, вызвавщей событие
		/// </summary>
		public HistoryNavigationType NavigationType
		{
			get { return _NavType; }
		}
	}

	/// <summary>
	/// Тип обработчика события MessageNavigate
	/// </summary>
	public delegate void MessageNavigateEventHandler(object sender, MessageNavigateEventArgs e);
}