using System;
using System.Diagnostics;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Extensibility;

using Rsdn.Janus.CoreServices.JBrowser;

namespace Rsdn.Janus
{
	//ToDo: перенести класс в Janus-Common, когда появится новый механизм конфига
	public static class BrowserHelper
	{
		public static void OpenUrlInBrowser(this IServiceProvider provider, string url)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException(nameof(url));

			var cfg = provider.GetRequiredService<IBrowserConfigService>();
			var urlsBehavior = new Uri(url).Scheme.Equals(
					JanusProtocolConstants.JanusUriScheme,
					StringComparison.InvariantCultureIgnoreCase)
				? UrlBehavior.InternalBrowser
				: cfg.Behavior;

			provider.OpenUrlInBrowser(url, urlsBehavior);
		}

		public static void OpenUrlInBrowser(
			this IServiceProvider provider, string url, UrlBehavior behavior)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException(nameof(url));

			if (behavior == UrlBehavior.InternalBrowser)
				provider.GetRequiredService<IBrowserService>().OpenUrl(url);
			else
				try
				{
					Process.Start(url);
				}
				catch (Exception ex)
				{
					var messageResult = MessageBox.Show(
						provider.GetRequiredService<IUIShell>().GetMainWindowParent(),
						JBrowserResources.OpenExternalBrowserException.FormatWith(ex),
						JBrowserResources.OpenExternalBrowserMsgBoxTitle,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Error);

					if (messageResult == DialogResult.Yes)
						provider.GetRequiredService<IBrowserService>().OpenUrl(url);
				}
		}
	}
}