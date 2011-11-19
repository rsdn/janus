namespace Rsdn.Janus
{
	public interface INotificationService
	{
		/// <summary>
		/// Показать уведомление.
		/// </summary>
		void ShowNotification(string title, string text, NotificationType notificationType, int timeout);
	}
}