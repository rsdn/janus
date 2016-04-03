using System;

using CodeJam.Extensibility;

namespace Rsdn.Janus.Framework
{
	internal class DefaultProgressService : IProgressService
	{
		private readonly IServiceProvider _serviceProvider;
		private ProgressForm _progressForm;

		public DefaultProgressService(IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));

			_serviceProvider = provider;

			_serviceProvider
				.GetRequiredService<IUIShell>()
				.CreateUIAsyncOperation()
				.Send(() => _progressForm = new ProgressForm(_serviceProvider));
		}

		#region IProgressService Members

		public IProgressVisualizer CreateVisualizer(bool allowCancel)
		{
			return _progressForm.CreateVisualizer(allowCancel);
		}

		#endregion
	}
}