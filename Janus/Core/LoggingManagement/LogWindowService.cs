using System;

using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[Service(typeof(ILogWindowService))]
	internal sealed class LogWindowService : ILogWindowService, IDisposable
	{
		private readonly IServiceProvider _serviceProvider;
		private LogForm _logForm;

		public LogWindowService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			_serviceProvider = serviceProvider;
		}

		public void Show()
		{
			GetLogForm().EnsureVisible();
		}

		public void Close()
		{
			if (_logForm != null && !_logForm.IsDisposed)
				_logForm.Close();
		}

		public void Clear()
		{
			GetLogForm().Clear();
		}

		public void Dispose()
		{
			Close();
		}

		private LogForm GetLogForm()
		{
			if (_logForm == null || _logForm.IsDisposed)
				_logForm = new LogForm(_serviceProvider);
			return _logForm;
		}
	}
}