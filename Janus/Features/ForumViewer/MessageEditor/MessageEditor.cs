namespace Rsdn.Janus
{
	/// <summary>
	/// Редактор сообщений.
	/// </summary>
	public static class MessageEditor
	{
		public static void EditMessage(MessageFormMode mode, MessageInfo msgInfo)
		{
			new MessageForm(ApplicationManager.Instance.ServiceProvider, mode, msgInfo).Show();
		}
	}
}
