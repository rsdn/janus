using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Редактор сообщений.
	/// </summary>
	public static class MessageEditor
	{
		public static void EditMessage(IServiceProvider provider, MessageFormMode mode, MessageInfo msgInfo)
		{
			new MessageForm(provider, mode, msgInfo).Show();
		}
	}
}
