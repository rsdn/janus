using System;
using System.Linq;

using LinqToDB;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Редактор новых сообщений.
	/// </summary>
	internal class NewMessageEditor : IOutboxItemEditor
	{
		private readonly IServiceProvider _provider;

		public NewMessageEditor(IServiceProvider provider)
		{
			_provider = provider;
		}

		#region IOutboxItemEditor Members
		public bool AllowEdit(object item)
		{
			return true;
		}

		public bool AllowDelete(object item)
		{
			return true;
		}

		public void Edit(object item)
		{
			MessageInfo info;
			var id = ((NewMessage)item).ID;
			using (var db = _provider.CreateDBContext())
			{
				var msg = db.OutboxMessages(m => m.ID == id);
				info =
					msg
						.Select(
							m =>
								new MessageInfo(
									m.ID,
									m.ForumID,
									m.ReplyToID,
									m.Subject,
									m.Body))
						.Single();
				msg.Set(_ => _.Hold, _ => true).Update();
			}
			MessageEditor.EditMessage(
				_provider,
				MessageFormMode.Edit,
				info);
			_provider
				.GetRequiredService<IOutboxManager>()
				.NewMessages
				.Refresh();
		}

		public void Delete(object item)
		{
			((NewMessage)item).Delete();
		}
		#endregion
	}
}
