using System;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обеспечивает запоминание истории просмотра сообщений и навигацию по ней
	/// </summary>
	public class MessageViewHistory
	{
		private readonly List<MessageViewHistoryEntry> _backPath = new List<MessageViewHistoryEntry>();
		private readonly List<MessageViewHistoryEntry> _forwardPath = new List<MessageViewHistoryEntry>();
		private MessageViewHistoryEntry _currentEntry;

		/// <summary>
		/// Обратный путь
		/// </summary>
		public IList<MessageViewHistoryEntry> BackPath
		{
			get { return _backPath; }
		}

		/// <summary>
		/// Прямой путь
		/// </summary>
		public IList<MessageViewHistoryEntry> ForwardPath
		{
			get { return _forwardPath; }
		}

		/// <summary>
		/// Текущая позиция
		/// </summary>
		public MessageViewHistoryEntry CurrentEntry
		{
			get { return _currentEntry; }
		}

		/// <summary>
		/// Происходит при навигации на новое сообщение. Обработчик должен установить 
		/// в аргументах тему сообщения.
		/// </summary>
		public event MessageNavigateEventHandler MessageNavigate;

		/// <summary>
		/// Осуществляет вызов события MessageNavigate
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMessageNavigate(MessageNavigateEventArgs e)
		{
			if (MessageNavigate != null)
				MessageNavigate(this, e);
		}

		/// <summary>
		/// Перейти на новое сообщение
		/// </summary>
		/// <param name="msgId">идентификатор сообщения</param>
		public void Navigate(int msgId)
		{
			if (_currentEntry != null && _currentEntry.MessageId == msgId)
				return;

			if (_currentEntry != null)
				_backPath.Insert(0, _currentEntry);
			_forwardPath.Clear();

			var e = new MessageNavigateEventArgs(
				msgId, HistoryNavigationType.Navigate);
			OnMessageNavigate(e);

			_currentEntry = new MessageViewHistoryEntry(msgId, e.MessageSubject);
		}

		/// <summary>
		/// Откатится назад
		/// </summary>
		public void Back()
		{
			if (_backPath.Count > 0)
			{
				if (_currentEntry != null)
					_forwardPath.Insert(0, _currentEntry);

				_currentEntry = _backPath[0];
				_backPath.RemoveAt(0);

				OnMessageNavigate(new MessageNavigateEventArgs(
					_currentEntry.MessageId, HistoryNavigationType.Back));
			}
		}

		/// <summary>
		/// Откатится назад до указанной точки
		/// </summary>
		/// <param name="entry">точка останова отката</param>
		public void Back(MessageViewHistoryEntry entry)
		{
			var idx = _backPath.IndexOf(entry);
			if (idx == -1)
				throw new ArgumentException("Requested entry not found in history", "entry");

			var movedEntries = ExtractRange(_backPath, idx);

			if (_currentEntry != null)
				_forwardPath.Insert(0, _currentEntry);

			foreach (var ery in movedEntries)
				_forwardPath.Insert(0, ery);

			_currentEntry = _backPath[0];
			_backPath.RemoveAt(0);

			OnMessageNavigate(new MessageNavigateEventArgs(
				_currentEntry.MessageId, HistoryNavigationType.Back));
		}

		/// <summary>
		/// Отменить откат
		/// </summary>
		public void Forward()
		{
			if (_forwardPath.Count > 0)
			{
				if (_currentEntry != null)
					_backPath.Insert(0, _currentEntry);

				_currentEntry = _forwardPath[0];
				_forwardPath.RemoveAt(0);

				OnMessageNavigate(new MessageNavigateEventArgs(
					_currentEntry.MessageId, HistoryNavigationType.Forward));
			}
		}

		/// <summary>
		/// Отменить откат до указанной точки
		/// </summary>
		/// <param name="entry">точка останова отмены отката</param>
		public void Forward(MessageViewHistoryEntry entry)
		{
			var idx = _forwardPath.IndexOf(entry);
			if (idx == -1)
				throw new ArgumentException("Requested entry not found in history", "entry");

			var movedEntries = ExtractRange(_forwardPath, idx);

			if (_currentEntry != null)
				_backPath.Insert(0, _currentEntry);

			foreach (var ery in movedEntries)
				_backPath.Insert(0, ery);

			_currentEntry = _forwardPath[0];
			_forwardPath.RemoveAt(0);

			OnMessageNavigate(new MessageNavigateEventArgs(
				_currentEntry.MessageId, HistoryNavigationType.Forward));
		}

		private static List<MessageViewHistoryEntry> ExtractRange(IList<MessageViewHistoryEntry> list, int count)
		{
			var movedEntries = new List<MessageViewHistoryEntry>();
			for (var i = 0; i < count; i++)
			{
				movedEntries.Add(list[0]);
				list.RemoveAt(0);
			}
			return movedEntries;
		}
	}
}