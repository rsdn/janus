using System;

using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[Service(typeof(IBrowserService))]
	internal class JBrowserService : IBrowserService
	{
		private readonly IServiceProvider _serviceProvider;
		private WebBrowserForm _webBrowserForm;

		public JBrowserService([NotNull] IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));

			_serviceProvider = provider;
		}

		#region IBrowserService Members

		public void OpenUrl([NotNull] string url)
		{
			if (url == null)
				throw new ArgumentNullException(nameof(url));

			if (_webBrowserForm == null || _webBrowserForm.IsDisposed)
				_webBrowserForm = new WebBrowserForm(_serviceProvider);

			_webBrowserForm.EnsureVisible();
			_webBrowserForm.NavigateTo(url);
		}

		#endregion
	}
}