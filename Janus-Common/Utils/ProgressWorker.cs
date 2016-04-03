using System;
using System.Windows.Forms;

using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Позволяет выполнить длительную работу в фоновом режиме,
	/// при этом на экране будет показываться визуализатор прогресса,
	/// а пользовательский интерфейс будет заблокирован.
	/// </summary>
	public static class ProgressWorker
	{
		public static void Run(
			[NotNull] IServiceProvider provider,
			bool allowCancel,
			[NotNull] Action<IProgressVisualizer> work)
		{
			Run(provider, allowCancel, work, null);
		}

		public static void Run(
			[NotNull] IServiceProvider provider,
			bool allowCancel,
			[NotNull] Action<IProgressVisualizer> work,
			[CanBeNull] Action completeHandler)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));
			if (work == null)
				throw new ArgumentNullException(nameof(work));

			IProgressVisualizer progressVisualizer = null;

			AsyncHelper.RunAsync(
				asyncOp =>
				{
					progressVisualizer = provider
						.GetRequiredService<IProgressService>()
						.CreateVisualizer(allowCancel);

					work(progressVisualizer);
				},
				() =>
				{
					progressVisualizer.Complete();
					completeHandler?.Invoke();
				});

			Application.DoEvents();
		}
	}
}