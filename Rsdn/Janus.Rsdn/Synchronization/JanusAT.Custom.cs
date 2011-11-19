using System;
using System.Net;
using Rsdn.Janus.AT;
using Rsdn.Janus.Log;
using Rsdn.Janus.Properties;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal sealed class JanusATCustom : JanusAT, ITransferNotificator
	{
		private readonly IServiceProvider _provider;
		private readonly Func<IWebConnectionConfig> _syncConfigGetter;
		private int _total;
		private int _current;

		public JanusATCustom(IServiceProvider provider, Func<IWebConnectionConfig> syncConfigGetter)
		{
			_provider = provider;
			_syncConfigGetter = syncConfigGetter;
		}

		public int TotalUploaded { get; private set; }
		public int TotalDownloaded { get; private set; }
		public event TransferProgressHandler TransferProgress;
		public event TransferBeginHandler TransferBegin;
		public event TransferCompleteHandler TransferComplete;

		private void OnTransferBegin(int total, TransferDirection direction, CompressionState state)
		{
			if (TransferBegin != null)
				TransferBegin(total, direction, state);
		}

		private void OnTransferProgress(int total, int current, TransferDirection direction)
		{
			if (TransferProgress != null)
				TransferProgress(total, current, direction);
		}

		private void OnTransferComplete(int total, TransferDirection direction)
		{
			if (TransferComplete != null)
				TransferComplete(total, direction);
		}

		///<summary>
		///Creates a <see cref="T:System.Net.WebRequest" /> for the specified <paramref name="uri" />.
		///</summary>
		protected override WebRequest GetWebRequest(Uri uri)
		{
			var rq = (HttpWebRequest)base.GetWebRequest(uri);
			var syncCfg = _syncConfigGetter();
			if (syncCfg.UseCompression)
				rq.Headers["Accept-Encoding"] = "gzip,deflate";

			// Для работы через прокси c отличной от NTLM аутентификацией.
			if (syncCfg.ProxyConfig.UseCustomAuthProxy)
				rq.KeepAlive = false;
			return rq;
		}

		#region Хуки для отлова длины входящего SOAP сообщения
		private WebResponse GetResponseInternal(WebRequest request)
		{
			// сбрасываем счетчики перед запросом
			_total = (int)request.ContentLength;
			_current = 0;
			OnTransferBegin(_total, TransferDirection.Send, CompressionState.Off);

			var response = (HttpWebResponse)base.GetWebResponse(request);
			if (response == null)
				return null;
			_provider.LogInfo(
				"JanusAT: {0}".FormatStr(
					!response.ContentEncoding.IsNullOrEmpty()
						? Resources.CompressionUsed.FormatStr(response.ContentEncoding)
						: Resources.NoCompression));

			TotalUploaded += _total;
			OnTransferComplete(_total, TransferDirection.Send);

			// сбрасываем счетчики перед получением результата
			_total = (int)response.ContentLength;
			_current = 0;
			OnTransferBegin(
				_total,
				TransferDirection.Receive,
				response.ContentEncoding.IsNullOrEmpty() ? CompressionState.Off : CompressionState.On);

			var proxy =
				new WebResponseProxy<HttpWebResponse>(
					response,
					stream => stream.WrapStream(
						bytes =>
							{
								_current += bytes;
								OnTransferProgress(_total, _current, TransferDirection.Receive);
							},
						() =>
							{
								OnTransferComplete(_total, TransferDirection.Receive);
								TotalDownloaded += _total > 0 ? _total : _current;
							},
						response.ContentEncoding));

			return proxy;
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			return GetResponseInternal(request);
		}

		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			return GetResponseInternal(request);
		}
		#endregion
	}
}


