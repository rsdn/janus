using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Services.Protocols;

using CodeJam;
using CodeJam.Collections;
using CodeJam.Extensibility;

using Rsdn.Janus.Log;
using Rsdn.Janus.Synchronization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Базовая реализация провайдера синхронизации с веб-сервисом.
	/// </summary>
	public abstract class WebServiceSyncProvider<T> : ISyncProvider
		where T : SoapHttpClientProtocol
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly Dictionary<string, IWebSvcSyncTask<T>> _tasks
			= new Dictionary<string, IWebSvcSyncTask<T>>();
		private readonly IWebConnectionService _webConSvc;

		protected WebServiceSyncProvider(
			IServiceProvider provider,
			IEnumerable<IWebSvcSyncTask<T>> syncTasks)
		{
			_serviceProvider = provider;
			_webConSvc = provider.GetRequiredService<IWebConnectionService>();
			foreach (var task in syncTasks)
				_tasks.Add(task.Name, task);
		}

		protected IServiceProvider ServiceProvider => _serviceProvider;

		protected IWebConnectionService WebConnectionService => _webConSvc;

		protected abstract T CreateServiceInstance();

		protected abstract void CallSvcCheckMethod(T svc);

		protected abstract string[] GetPeriodicTaskNames();

		/// <summary>
		/// Создать и инициализировать экземпляр сервиса.
		/// </summary>
		protected T CreateAndInitService()
		{
			var svc = CreateServiceInstance();
			svc.Timeout = _webConSvc.GetConfig().HttpTimeout;
			svc.CookieContainer = new CookieContainer();
			InitProxy(svc);

			return svc;
		}

		protected static void InitTransferProgress(ISyncContext context, T svc)
		{
			var notificator = svc as ITransferNotificator;
			if (notificator == null)
				return;
			var progressProv = context.GetService<ISyncProgressVisualizer>();
			if (progressProv == null)
				return;
			notificator.TransferBegin +=
				(total, direction, state) =>
				{
					context.CheckState();
					progressProv.ReportProgress(total, 0);
					var progressTextFormat = (direction == TransferDirection.Receive
						? SyncResources.Receiving
						: SyncResources.Sending) + " {0}/{1}";
					progressProv.SetProgressText(progressTextFormat.FormatWith(0, total.ToInfoSizeString()));
					progressProv.SetCompressionSign(state);
				};
			notificator.TransferProgress +=
				(total, current, direction) =>
				{
					context.CheckState();
					progressProv.ReportProgress(total, current);
					var progressTextFormat = (direction == TransferDirection.Receive
						? SyncResources.Receiving
						: SyncResources.Sending) + " {0}/{1}";
					progressProv.SetProgressText(
						progressTextFormat.FormatWith(current.ToInfoSizeString(), total.ToInfoSizeString()));
				};
			notificator.TransferComplete +=
				(total, direction) => progressProv.SetCompressionSign(CompressionState.Off);
		}

		private static void AddTrafficStats(ISyncContext ctx, T svc)
		{
			var notificator = svc as ITransferNotificator;
			if (notificator == null)
				return;
			ctx.AddUploadStats(notificator.TotalUploaded);
			ctx.AddDownloadStats(notificator.TotalDownloaded);
		}

		public bool IsAvailable()
		{
			try
			{
				using (var svc = CreateAndInitService())
					CallSvcCheckMethod(svc);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void SyncTask(ISyncContext context, string taskName)
		{
			IWebSvcSyncTask<T> task;
			if (!_tasks.TryGetValue(taskName, out task))
				throw new ArgumentException(@"Unknown task", nameof(taskName));
			if (!task.IsTaskActive())
				throw new ApplicationException("Specified task not active now.");
			PerformSyncProvider(context,
				svc =>
					task.Sync(
						context,
						svc,
						_webConSvc.GetConfig().RetriesCount,
						context.AppendTaskIndicator(task.GetDisplayName())));
		}

		/// <summary>
		/// Запустить периодические задачи.
		/// </summary>
		public void SyncPeriodicTasks(ISyncContext context)
		{
			PerformSyncProvider(context,
				svc =>
					GetPeriodicTaskNames()
						.Select(name => _tasks[name]) // Выбираем все периодические задачи
						.Where(task => task.IsTaskActive()) // Отфильтровываем неактивные
						.Select(
							task => new
							{
								Task = task,
								Indicator = context.AppendTaskIndicator(task.GetDisplayName())
							}) // Создаем индикаторы
						.ToArray() // Сразу выполняем. чтобы все индикаторы добавились до начала синхронизации
						.ForEach(
						data =>
						{
							data.Task.Sync(context, svc, _webConSvc.GetConfig().RetriesCount, data.Indicator);
							context.CheckState();
						}));
		}

		private void PerformSyncProvider(ISyncContext context, Action<T> runner)
		{
			using (var svc = CreateAndInitService())
			{
				_serviceProvider.LogInfo(SyncResources.StartWith.FormatWith(svc.Url));
				InitTransferProgress(context, svc);
				runner(svc);
				AddTrafficStats(context, svc);
				OnSyncSessionFinished(svc);
			}
		}

		protected virtual void OnSyncSessionFinished(T svc)
		{}

		#region Авторизация на прокси-серверах
		protected virtual void InitProxy(T svc)
		{
			var pxyCfg = _webConSvc.GetConfig().ProxyConfig;
			switch (pxyCfg.UseProxyType)
			{
				case UseProxyType.UseIESettings:
					svc.Proxy = WebRequest.GetSystemWebProxy();
					svc.Proxy.Credentials = CredentialCache.DefaultCredentials;
					break;
				case UseProxyType.UseCustomSettings:
					svc.Proxy = new WebProxy(pxyCfg.ProxySettings.ProxyUri)
					{
						Credentials = CredentialCache.DefaultCredentials
					};
					break;
			}

			if (svc.Proxy == null || !pxyCfg.UseCustomAuthProxy)
				return;

			_serviceProvider.LogInfo(SyncResources.TestDemandProxyAuth.FormatWith(pxyCfg.ProxySettings.ProxyUri));

			Action check = () => CallSvcCheckMethod(svc);
			var creds = _webConSvc.ProxyDemandAuth(check, svc.Abort);
			if (creds == null)
				return;
			svc.Proxy.Credentials = creds;
			_webConSvc.AuthOnProxy(check, svc.Abort);
		}
		#endregion
	}
}