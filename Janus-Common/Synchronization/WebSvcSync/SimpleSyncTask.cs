using System;
using System.Web.Services.Protocols;

using Rsdn.Janus.Synchronization;

namespace Rsdn.Janus
{
	public abstract class SimpleSyncTask<TSvc, TRq, TRsp> : IWebSvcSyncTask<TSvc>
		where TSvc : SoapHttpClientProtocol
		where TRq : class
	{
		private readonly Func<string> _displayNameGetter;
		private readonly string _name;

		/// <summary>
		/// Инициализирует экземпляр
		/// </summary>
		/// <param name="name">уникальное в пределах провайдера имя задачи</param>
		/// <param name="displayNameGetter">
		/// Делегат, возвращающий отображаемое имя задачи. Делегат используется,
		/// потому что имя зависит от локали потока, а она может
		/// меняться уже после конструирования.
		/// </param>
		protected SimpleSyncTask(
			string name,
			Func<string> displayNameGetter)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (displayNameGetter == null)
				throw new ArgumentNullException("displayNameGetter");

			_name = name;
			_displayNameGetter = displayNameGetter;
		}

		#region IWebSvcSyncTask<TSvc> Members
		/// <summary>
		/// Имя задачи.
		/// </summary>
		public virtual string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Отображаемое имя задачи.
		/// </summary>
		public string GetDisplayName()
		{
			return _displayNameGetter();
		}

		public abstract bool IsTaskActive();

		/// <summary>
		/// Выполнить синхронизацию.
		/// </summary>
		public void Sync(
			ISyncContext context,
			TSvc svc,
			int retries,
			ITaskIndicator indicator)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (svc == null)
				throw new ArgumentNullException("svc");
			if (indicator == null)
				throw new ArgumentNullException("indicator");

			indicator.SetTaskState(SyncTaskState.Sync);
			try
			{
				context.CheckState();

				indicator.SetStatusText(SyncResources.IndicatorPrepareRequest);
				var rq = PrepareRequest(context);
				if (rq == null)
					return; // Nothing to do

				context.CheckState();

				indicator.SetStatusText(SyncResources.IndicatorMakeRequest);
				var rsp = context.CallWithRetries(rq, GetDisplayName(), retries,
					() => MakeRequest(context, svc, rq));

				context.CheckState();

				indicator.SetStatusText(SyncResources.IndicatorProcessResponse);
				ProcessResponse(context, rq, rsp);

				context.CheckState();

				indicator.SetTaskState(SyncTaskState.Succeed);
				indicator.SetStatusText(SyncResources.IndicatorSuccessFinish);
			}
			// Отмену пользователем не трактуем как ошибку и выкидываем наверх
			catch (UserCancelledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				indicator.SetTaskState(SyncTaskState.Failed);
				indicator.SetStatusText(ex.Message);
				context.TryAddSyncError(
					new SyncErrorInfo(SyncErrorType.CriticalError, GetDisplayName(), ex.ToString()));
			}
		}
		#endregion

		protected abstract TRq PrepareRequest(ISyncContext context);
		protected abstract TRsp MakeRequest(ISyncContext context, TSvc svc, TRq rq);
		protected abstract void ProcessResponse(ISyncContext context, TRq request, TRsp response);
	}
}