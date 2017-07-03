using System;
using System.IO;
using System.IO.Compression;

using CodeJam;
using CodeJam.Strings;

using JetBrains.Annotations;

using Rsdn.Janus.Log;
using Rsdn.Janus.Synchronization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Вспомогательный класс для синхронизации по сети.
	/// </summary>
	public static class NetworkSyncHelper
	{
		public static void AddUploadStats(this ISyncContext context, int bytesCount)
		{
			context.StatisticsContainer.AddValue(NetworkSyncInfo.UploadTrafficStats, bytesCount);
		}

		public static void AddDownloadStats(this ISyncContext context, int bytesCount)
		{
			context.StatisticsContainer.AddValue(NetworkSyncInfo.DownloadTrafficStats, bytesCount);
		}

		public static TRsp CallWithRetries<TRq, TRsp>(
			this ISyncContext context,
			TRq request,
			string taskName,
			int retriesCount,
			Func<TRsp> requestMaker)
		{
			var resp = default(TRsp);
			var retries = retriesCount + 1;
			while (retries-- > 0)
			{
				try
				{
					resp = requestMaker();
				}
					// Отмену пользователем не трактуем как ошибку и выкидываем наверх
				catch (UserCancelledException)
				{
					throw;
				}
				catch (Exception ex)
				{
					if (retries > 0)
					{
						context.LogWarning(SyncResources.ErrorMessage.FormatWith(ex.Message));
						context.TryAddSyncError(
							new SyncErrorInfo(SyncErrorType.Warning, taskName, ex.ToString()));
						context.CheckState();
						continue;
					}
					throw;
				}
				break;
			}
			return resp;
		}

		public static Stream WrapStream(
			[NotNull] this Stream baseStream,
			[NotNull] Action<int> progressHandler,
			Action closeHandler,
			string compression)
		{
			if (baseStream == null)
				throw new ArgumentNullException(nameof(baseStream));
			if (progressHandler == null)
				throw new ArgumentNullException(nameof(progressHandler));

			var trackedStream = new TrackedStream(baseStream, closeHandler);
			trackedStream.BytesTransferred += progressHandler;
			Stream newStream = trackedStream;
			if (compression == "gzip")
				newStream = new GZipStream(newStream, CompressionMode.Decompress);
			else if (compression == "deflate")
				newStream = new DeflateStream(newStream, CompressionMode.Decompress);
			return newStream;
		}

		#region TrackedStream class
		private class TrackedStream : Stream
		{
			private readonly Stream _baseStream;
			private readonly Action _closeHandler;

			public TrackedStream(Stream baseStream, Action closeHandler)
			{
				_baseStream = baseStream;
				_closeHandler = closeHandler;
			}

			public event Action<int> BytesTransferred;

			private void OnBytesTransferred(int bytesCount)
			{
				BytesTransferred?.Invoke(bytesCount);
			}

			///<summary>
			/// When overridden in a derived class, clears all buffers for
			/// this stream and causes any buffered data to be written to
			/// the underlying device.
			///</summary>
			public override void Flush()
			{
				_baseStream.Flush();
			}

			///<summary>
			///When overridden in a derived class, sets the position within the current stream.
			///</summary>
			public override long Seek(long offset, SeekOrigin origin)
			{
				return _baseStream.Seek(offset, origin);
			}

			///<summary>
			///When overridden in a derived class, sets the length of the current stream.
			///</summary>
			public override void SetLength(long value)
			{
				_baseStream.SetLength(value);
			}

			///<summary>
			/// When overridden in a derived class, reads a sequence of bytes from
			/// the current stream and advances the position within the stream by
			/// the number of bytes read.
			///</summary>
			public override int Read(byte[] buffer, int offset, int count)
			{
				var bytesCount = _baseStream.Read(buffer, offset, count);
				OnBytesTransferred(bytesCount);
				return bytesCount;
			}

			///<summary>
			/// When overridden in a derived class, writes a sequence of bytes
			/// to the current stream and advances the current position within
			/// this stream by the number of bytes written.
			///</summary>
			public override void Write(byte[] buffer, int offset, int count)
			{
				_baseStream.Write(buffer, offset, count);
				OnBytesTransferred(count);
			}

			///<summary>
			///When overridden in a derived class, gets a value indicating
			/// whether the current stream supports reading.
			///</summary>
			public override bool CanRead => _baseStream.CanRead;

			///<summary>
			/// When overridden in a derived class, gets a value indicating
			/// whether the current stream supports seeking.
			///</summary>
			public override bool CanSeek => _baseStream.CanSeek;

			///<summary>
			/// When overridden in a derived class, gets a value indicating whether
			/// the current stream supports writing.
			///</summary>
			public override bool CanWrite => _baseStream.CanWrite;

			///<summary>
			///When overridden in a derived class, gets the length in bytes of the stream.
			///</summary>
			public override long Length => _baseStream.Length;

			///<summary>
			/// When overridden in a derived class, gets or sets the position within
			/// the current stream.
			///</summary>
			public override long Position
			{
				get { return _baseStream.Position; }
				set { _baseStream.Position = value; }
			}
			public override void Close()
			{
				base.Close();
				_closeHandler?.Invoke();
			}

			protected override void Dispose(bool disposing)
			{
				try
				{
					if (disposing)
						_baseStream.Dispose();
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}
		#endregion
	}
}