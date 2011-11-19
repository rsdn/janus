using System;
using System.Net;
using System.Windows.Forms;
using Rsdn.Janus.Core.Synchronization;
using Rsdn.Janus.Log;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof (IWebConnectionService))]
	internal class WebConnectionService : IWebConnectionService
	{
		private readonly IServiceProvider _provider;
		private readonly CredentialCache _authCache = new CredentialCache();

		public WebConnectionService(IServiceProvider provider)
		{
			_provider = provider;
		}

		#region Proxy auth
		public void AuthOnProxy(Action check, Action abort)
		{
			var authComplete = false;
			var cfg = Config.Instance;
			var retriesProxy = cfg.RetriesCount + 1;
			var firstStep = true;
			while (retriesProxy-- > 0)
			{
				if (ProxyDemandAuth(check, abort) != null)
				{
					_provider.LogWarning(SyncResources.ProxyDemandAuth);

					_provider.LogInfo(
						SyncResources.ProxyAuthOn
							.FormatStr(cfg.ProxyConfig.ProxySettings.ProxyUri));

					if (!firstStep)
						cfg.ProxyConfig.ProxySettings.IsPasswordAlreadySaved = false;

					var nc = GetCredential();
					if (nc == null)
						break;
					UpdateCredentialsCache(nc);
				}
				else
				{
					authComplete = true;
					break;
				}
				firstStep = false;
			}

			if (!authComplete)
			{
				var error = SyncResources.AuthOnProxyIsFailed
					.FormatStr(cfg.ProxyConfig.ProxySettings);
				_provider.LogError(error);
				throw new ApplicationException(error);
			}
			var strLog = SyncResources.AuthOnProxyIsComplete.FormatStr(cfg.ProxyConfig.ProxySettings);
			_provider.LogInfo(strLog);
		}

		public IWebConnectionConfig GetConfig()
		{
			var cfg = Config.Instance;
			return
				new WebConnectionConfig(
					cfg.UseCompression,
					cfg.HttpTimeout,
					cfg.RetriesCount,
					cfg.ProxyConfig);
		}

		public ICredentials ProxyDemandAuth(Action check, Action abort)
		{
			var isAuthProxy = false;

			try
			{
				check();
			}
			catch (WebException we)
			{
				abort();
				if (we.InnerException is WebException)
				{
					var hwr = ((WebException)we.InnerException).Response as HttpWebResponse;
					if (hwr != null)
						if (hwr.StatusCode == HttpStatusCode.ProxyAuthenticationRequired)
							isAuthProxy = true;
				}
			}

			return isAuthProxy ? _authCache : null;
		}

		private NetworkCredential GetCredential()
		{
			return
				Config.Instance.ProxyConfig.ProxySettings.IsPasswordAlreadySaved
					? GetSavedCredential()
					: GetUserCredential();
		}

		private static NetworkCredential GetSavedCredential()
		{
			var pxyCfg = Config.Instance.ProxyConfig;
			var credentials =
				new NetworkCredential
				{
					UserName = pxyCfg.ProxySettings.Login,
					Password = pxyCfg.ProxySettings.EncodedPassword.DecryptPassword()
				};
			return credentials;
		}

		private NetworkCredential GetUserCredential()
		{
			var credentials = new NetworkCredential();
			var pxyCfg = Config.Instance.ProxyConfig;
			using (var authForm = new ProxyAuthForm(pxyCfg))
			{
				if (authForm.ShowDialog(_provider.GetRequiredService<IUIShell>().GetMainWindowParent()) !=
					DialogResult.OK)
				{
					_provider.LogWarning(SyncResources.ProxyUserDeclineAuth);
					return null;
				}

				credentials.UserName = authForm.Login;
				credentials.Password = authForm.Password;
				pxyCfg.ProxySettings.Login = credentials.UserName;
				if (pxyCfg.ProxySettings.SaveAuth)
					pxyCfg.ProxySettings.EncodedPassword = credentials.Password.EncryptPassword();
			}
			return credentials;
		}

		private void UpdateCredentialsCache(NetworkCredential credentials)
		{
			var pxyCfg = Config.Instance.ProxyConfig;
			_authCache.Remove(pxyCfg.ProxySettings.ProxyUri, "Basic");
			_authCache.Add(pxyCfg.ProxySettings.ProxyUri, "Basic", credentials);
		}
		#endregion
	}
}