using System;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(INotificationService))]
	internal sealed class NotificationService : INotificationService
	{
		private readonly IServiceProvider _serviceProvider;

		public NotificationService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			_serviceProvider = serviceProvider;
		}

		#region Implementation of INotificationService

		public void ShowNotification(
			string title,
			[NotNull] string text,
			NotificationType notificationType,
			int timeout)
		{
			if (text.IsNullOrEmpty())
				throw new ArgumentException(
					"Текст оповещения не должен быть null или пустой строкой.", "text");

			_serviceProvider
				.GetRequiredService<INotifyIconService>()
				.ShowBalloonTip(title, text, notificationType, timeout);
		}

		#endregion
	}
}