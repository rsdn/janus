using System;
using System.IO;
using System.Net;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class WebResponseProxy<T> : WebResponse
		where T : WebResponse
	{
		private readonly T _baseResponse;
		private readonly Func<Stream, Stream> _streamWrapper;

		public WebResponseProxy(
			[NotNull] T baseResponse,
			[NotNull] Func<Stream, Stream> streamWrapper)
		{
			if (baseResponse == null)
				throw new ArgumentNullException("baseResponse");
			if (streamWrapper == null)
				throw new ArgumentNullException("streamWrapper");

			_baseResponse = baseResponse;
			_streamWrapper = streamWrapper;
		}

		public T BaseResponse
		{
			get { return _baseResponse; }
		}

		#region Overrides
		public override void Close()
		{
			_baseResponse.Close();
		}

		public override Stream GetResponseStream()
		{
			return _streamWrapper(_baseResponse.GetResponseStream());
		}

		public override bool IsFromCache
		{
			get { return _baseResponse.IsFromCache; }
		}

		public override bool IsMutuallyAuthenticated
		{
			get { return _baseResponse.IsMutuallyAuthenticated; }
		}

		public override long ContentLength
		{
			get { return _baseResponse.ContentLength; }
			set { _baseResponse.ContentLength = value; }
		}

		public override string ContentType
		{
			get { return _baseResponse.ContentType; }
			set { _baseResponse.ContentType = value; }
		}

		public override Uri ResponseUri
		{
			get { return _baseResponse.ResponseUri; }
		}

		public override WebHeaderCollection Headers
		{
			get { return _baseResponse.Headers; }
		}
		#endregion
	}
}