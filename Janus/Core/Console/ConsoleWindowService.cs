using System;

using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[Service(typeof(IConsoleWindowService))]
	internal sealed class ConsoleWindowService : IConsoleWindowService, IDisposable
	{
		private readonly IServiceProvider _serviceProvider;
		private ConsoleForm _consoleForm;

		public ConsoleWindowService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			_serviceProvider = serviceProvider;
		}

		public void Show()
		{
			GetConsoleForm().EnsureVisible();
		}

		public void Clear()
		{
			GetConsoleForm().Clear();
		}

		public void Close()
		{
			if (_consoleForm != null && !_consoleForm.IsDisposed)
				_consoleForm.Close();
		}

		public string PromptText
		{
			get { return GetConsoleForm().PromptText; }
			set { GetConsoleForm().PromptText = value; }
		}

		public void Dispose()
		{
			Close();
		}

		private ConsoleForm GetConsoleForm()
		{
			if (_consoleForm == null || _consoleForm.IsDisposed)
				_consoleForm = new ConsoleForm(_serviceProvider);
			return _consoleForm;
		}
	}
}