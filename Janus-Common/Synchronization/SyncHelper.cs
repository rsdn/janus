using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Вспомогательные методы для синхронизации.
	/// </summary>
	public static class SyncHelper
	{
		/// <summary>
		/// Синхронизировать, если это возможно.
		/// Выполняется асинхронно.
		/// </summary>
		public static void PeriodicSyncIfAvailable(
			this IServiceProvider provider,
			SyncThreadPriority priority,
			bool activateUI)
		{
			PeriodicSyncIfAvailable(provider, priority, null, activateUI);
		}

		/// <summary>
		/// Синхронизировать, если это возможно.
		/// Выполняется асинхронно.
		/// </summary>
		public static void PeriodicSyncIfAvailable(
			this IServiceProvider provider,
			SyncThreadPriority priority,
			SyncRequestFinishedHandler syncFinishedHandler,
			bool activateUI)
		{
			DoAsyncSync(
				sz => sz.SyncPeriodic(activateUI),
				provider,
				syncFinishedHandler,
				true,
				priority);
		}

		/// <summary>
		/// Синхронизировать.
		/// Выполняется асинхронно.
		/// </summary>
		public static void PeriodicSync(
			this IServiceProvider provider,
			SyncThreadPriority priority,
			bool activateUI)
		{
			PeriodicSync(provider, priority, null, activateUI);
		}

		/// <summary>
		/// Синхронизировать.
		/// Выполняется асинхронно.
		/// </summary>
		public static void PeriodicSync(
			this IServiceProvider provider,
			SyncThreadPriority priority,
			SyncRequestFinishedHandler syncFinishedHandler,
			bool activateUI)
		{
			DoAsyncSync(
				sz => sz.SyncPeriodic(activateUI),
				provider,
				syncFinishedHandler,
				false,
				priority);
		}

		/// <summary>
		/// Синхронизировать, если это возможно.
		/// Выполняется асинхронно.
		/// </summary>
		public static void SyncSpecific(
			this IServiceProvider provider,
			string providerName,
			string taskName,
			SyncThreadPriority priority,
			SyncRequestFinishedHandler syncFinishedHandler,
			bool activateUI)
		{
			DoAsyncSync(
				sz => sz.SyncSpecific(providerName, taskName, activateUI),
				provider,
				syncFinishedHandler,
				false,
				priority);
		}


		private static void DoAsyncSync(
			Func<ISynchronizer, IStatisticsContainer> syncProc,
			IServiceProvider provider,
			SyncRequestFinishedHandler syncFinishedHandler,
			bool checkAvailability,
			SyncThreadPriority priority)
		{
			var sz = provider.GetRequiredService<ISynchronizer>();
			if (!sz.IsActive())
			{
				var thread = CreateSyncThread(
					() =>
					{
						try
						{
							var syncPerformed = false;
							IStatisticsContainer stats = null;
							if (!checkAvailability || sz.IsAvailable())
							{
								syncPerformed = true;
								stats = syncProc(sz);
							}
							if (syncFinishedHandler != null)
								provider.GetRequiredService<IUIShell>()
									.CreateUIAsyncOperation()
									.PostOperationCompleted(state => syncFinishedHandler(syncPerformed, stats), null);
						}
						catch (Exception ex)
						{
							provider.GetRequiredService<IUIShell>()
								.CreateUIAsyncOperation()
								.PostOperationCompleted(
//{{{-PossibleIntendedRethrow
								state => { throw ex; }, null);
//{{{+PossibleIntendedRethrow
						}
					},
					priority);
				thread.Start();
			}
			else if (syncFinishedHandler != null)
				syncFinishedHandler(false, null);
		}

		private static Thread CreateSyncThread(
			ThreadStart method,
			SyncThreadPriority priority)
		{
			return new Thread(method)
			{
				IsBackground = true,
				CurrentCulture = Thread.CurrentThread.CurrentCulture,
				CurrentUICulture = Thread.CurrentThread.CurrentUICulture,
				Priority =
					priority == SyncThreadPriority.Normal
						? ThreadPriority.Normal
						: ThreadPriority.Lowest
			};
		}

		/// <summary>
		/// Добавляет индикатор задачи к существующему провайдеру индикаторов,
		/// либо возвращает пустой индикатор.
		/// </summary>
		public static ITaskIndicator AppendTaskIndicator(this IServiceProvider provider, string taskName)
		{
			var indSvc = provider.GetService<ITaskIndicatorProvider>();
			return
				indSvc != null
					? indSvc.AppendTaskIndicator(taskName)
					: new EmptyTaskIndicator();
		}

		/// <summary>
		/// Получить текст всех ошибок синхронизации.
		/// </summary>
		public static string GetText(this IEnumerable<SyncErrorInfo> errors)
		{
			var sb = new StringBuilder();
			foreach (var error in errors)
			{
				if (sb.Length > 0)
					sb.AppendLine("");
				sb.AppendLine("{0} - {1}".FormatStr(error.TaskName, error.Type));
				sb.AppendLine(error.Text);
			}
			return sb.ToString();
		}

		public static void TryAddSyncError(this IServiceProvider provider, SyncErrorInfo error)
		{
			var svc = provider.GetService<ISyncErrorInformer>();
			if (svc == null)
				return;
			svc.AddError(error);
		}
	}

	/// <summary>
	/// Обработчик завершения попытки синхронизации.
	/// </summary>
	public delegate void SyncRequestFinishedHandler(bool syncPerformed, IStatisticsContainer stats);
}