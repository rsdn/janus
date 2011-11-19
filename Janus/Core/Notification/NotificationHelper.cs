using System;

using Rsdn.SmartApp;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class NotificationHelper
	{
		public static void ShowNotification(
			[NotNull] this IServiceProvider provider,
			[NotNull] string text)
		{
			provider.ShowNotification(text, NotificationType.Default);
		}

		public static void ShowNotification(
			[NotNull] this IServiceProvider provider,
			[NotNull] string text,
			NotificationType notificationType)
		{
			provider.ShowNotification(null, text, notificationType);
		}

		public static void ShowNotification(
			[NotNull] this IServiceProvider provider,
			string title,
			[NotNull] string text,
			NotificationType notificationType)
		{
			provider.ShowNotification(
				title, 
				text, 
				notificationType, 
				Config.Instance.NotificationConfig.LateralTimeout);
		}

		public static void ShowNotification(
			[NotNull] this IServiceProvider provider,
			string title,
			[NotNull] string text,
			NotificationType notificationType,
			int timeout)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (!Config.Instance.NotificationConfig.LateralNotifier)
				return;

			var notificationService = provider.GetService<INotificationService>();
			if (notificationService != null)
				notificationService.ShowNotification(title, text, notificationType, timeout);
		}
	}
}