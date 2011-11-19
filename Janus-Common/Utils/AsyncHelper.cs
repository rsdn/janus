using System;
using System.ComponentModel;
using System.Threading;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Хелпер для работы с асинхронными задачами.
	/// </summary>
	public static class AsyncHelper
	{
		/// <summary>
		/// Аналог <see cref="AsyncOperationManager"/> без дурацкого state.
		/// </summary>
		public static AsyncOperation CreateOperation()
		{
			return AsyncOperationManager.CreateOperation(null);
		}

		/// <summary>
		/// Аналог <see cref="AsyncOperation.Post"/> без дурацкого state.
		/// </summary>
		public static void Post(
			[NotNull] this AsyncOperation asyncOp,
			[NotNull] Action runner)
		{
			if (asyncOp == null)
				throw new ArgumentNullException("asyncOp");
			if (runner == null)
				throw new ArgumentNullException("runner");

			asyncOp.Post(state => runner(), null);
		}

		/// <summary>
		/// Аналог <see cref="AsyncOperation.Post"/> без дурацкого state.
		/// </summary>
		public static void Post([NotNull] Action runner)
		{
			PostOperationCompleted(CreateOperation(), runner);
		}

		/// <summary>
		/// Аналог <see cref="AsyncOperation.Post"/> без дурацкого state.
		/// </summary>
		public static void PostOperationCompleted(
			[NotNull] this AsyncOperation asyncOp,
			[NotNull] Action runner)
		{
			if (asyncOp == null)
				throw new ArgumentNullException("asyncOp");
			if (runner == null)
				throw new ArgumentNullException("runner");

			asyncOp.PostOperationCompleted(state => runner(), null);
		}

		/// <summary>
		/// Отсутствующий в интерфейсе <see cref="AsyncOperation"/> метод Send.
		/// </summary>
		public static void Send(
			[NotNull] this AsyncOperation asyncOp,
			[NotNull] Action runner)
		{
			if (asyncOp == null)
				throw new ArgumentNullException("asyncOp");
			if (runner == null)
				throw new ArgumentNullException("runner");

			asyncOp.SynchronizationContext.Send(state => runner(), null);
		}

		public static TResult Send<TResult>(
			[NotNull] this AsyncOperation asyncOp,
			[NotNull] Func<TResult> valueGetter)
		{
			if (asyncOp == null)
				throw new ArgumentNullException("asyncOp");
			if (valueGetter == null)
				throw new ArgumentNullException("valueGetter");

			var result = default (TResult);
			asyncOp.Send(() => { result = valueGetter(); });
			return result;
		}

		/// <summary>
		/// Получить активный worker-поток из пула и создать <see cref="AsyncOperation"/>.
		/// </summary>
		public static void RunAsync([NotNull] Action<AsyncOperation> runner)
		{
			if (runner == null)
				throw new ArgumentNullException("runner");

			var asyncOp = CreateOperation();
			ThreadPool.QueueUserWorkItem(state => runner(asyncOp));
		}

		/// <summary>
		/// Получить активный worker-поток из пула и создать <see cref="AsyncOperation"/>.
		/// </summary>
		/// <remarks>
		/// <see cref="completeHandler"/> выполняется уже синхронизированным с вызвавшим потоком.
		/// </remarks>
		public static void RunAsync(
			[NotNull] Action<AsyncOperation> runner,
			[NotNull] Action completeHandler)
		{
			if (runner == null)
				throw new ArgumentNullException("runner");
			if (completeHandler == null)
				throw new ArgumentNullException("completeHandler");

			var asyncOp = CreateOperation();
			ThreadPool.QueueUserWorkItem(
				state =>
				{
					try
					{
						runner(asyncOp);
					}
					finally
					{
						asyncOp.PostOperationCompleted(completeHandler);
					}
				});
		}
	}
}