using System;
using System.Diagnostics;
using System.Windows.Forms;

using Rsdn.Janus.Core.JBrowserManagement;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	//ToDo: перенести класс в Janus-Common, когда появится новый механизм конфига
	public static class BrowserHelper
	{
		public static void OpenUrlInBrowser(this IServiceProvider provider, string url)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			if (string.IsNullOrEmpty(url))
				throw new ArgumentException("Аргумент не должен быть null или пустой строкой", "url");

			var urlsBehavior = new Uri(url).Scheme.Equals(
					JanusProtocolInfo.JanusUriScheme,
					StringComparison.InvariantCultureIgnoreCase)
				? UrlBehavior.InternalBrowser
				: Config.Instance.Behavior;

			provider.OpenUrlInBrowser(url, urlsBehavior);
		}

		public static void OpenUrlInBrowser(
			this IServiceProvider provider, string url, UrlBehavior behavior)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			if (string.IsNullOrEmpty(url))
				throw new ArgumentException("Аргумент не должен быть null или пустой строкой", "url");

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
						JBrowserResources.OpenExternalBrowserException.FormatStr(ex),
						JBrowserResources.OpenExternalBrowserMsgBoxTitle,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Error);

					if (messageResult == DialogResult.Yes)
						provider.GetRequiredService<IBrowserService>().OpenUrl(url);
				}
		}
	}
}