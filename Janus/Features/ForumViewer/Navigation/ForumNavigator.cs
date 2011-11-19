using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Осуществляет навигацию по форумам.
	/// </summary>
	public class ForumNavigator
	{
		private readonly MessageViewHistory _viewHistory = new MessageViewHistory();
		private readonly IServiceProvider _provider;

		internal ForumNavigator(IServiceProvider provider)
		{
			_provider = provider;
			_viewHistory.MessageNavigate += HistoryNavigateFired;
		}

		/// <summary>
		/// История просмотра.
		/// </summary>
		public MessageViewHistory ViewHistory
		{
			get { return _viewHistory; }
		}

		/// <summary>
		/// Текущее состояние - режим навигации.
		/// </summary>
		public bool NavigationMode { get; private set; }

		public event EventHandler MessageNavigated;

		/// <summary>
		/// Перейти на сообщение с указанным id.
		/// </summary>
		public bool SelectMessage(int msgId)
		{
			return SelectMessage(null, msgId);
		}

		/// <summary>
		/// Перейти на именованное сообщение.
		/// </summary>
		/// <param name="name">Имя сообщения.</param>
		/// <returns><c>true</c> - перейти удалось, иначе - <c>false</c>.</returns>
		public void SelectMessage(string name)
		{
			var found =
				DatabaseManager.GetMessageByName(
					_provider,
					name,
					msg => new {msg.ID, msg.ForumID});
			if (found != null)
				SelectMessage(found.ForumID, found.ID);
		}

		/// <summary>
		/// Перейти на сообщение с указанным id в указанном форуме.
		/// </summary>
		public bool SelectMessage(int? forumId, int msgId)
		{
			if (forumId == null)
				forumId = DatabaseManager.FindMessageForumId(_provider, msgId);

			if (forumId == null)
				return false;

			var forum = Forums.Instance[forumId.Value];

			if (forum == null)
				return false;

			var msg = forum.FindMsgById(msgId);
			
			if (msg == null)
				return false;

			NavigationMode = true;

			try
			{
				forum.ActiveMsg = msg;

				// Refresh forum only if changed
				if (Forums.Instance.ActiveForum == null 
					|| forumId != Forums.Instance.ActiveForum.ID)
				{
					Forums.Instance.ActiveForum = null;
					Forums.Instance.ActiveForum = forum;
				}

				_viewHistory.Navigate(msg.ID);
			}
			finally
			{
				NavigationMode = false;
			}

			return true;
		}

		private void HistoryNavigateFired(object sender, MessageNavigateEventArgs e)
		{
			if (!NavigationMode)
				SelectMessage(_viewHistory.CurrentEntry.MessageId);

			if (Forums.Instance.ActiveForum != null)
			{
				var msg = Forums.Instance.ActiveForum.ActiveMsg;
				if (msg != null)
					e.MessageSubject = msg.Subject;
			}

			if (MessageNavigated != null)
				MessageNavigated(this, EventArgs.Empty);
		}
	}
}