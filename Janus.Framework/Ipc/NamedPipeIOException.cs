using System;
using System.Runtime.Serialization;

namespace Rsdn.Janus.Framework.Ipc
{
	/// <summary>
	/// This exception is thrown by named pipes communication methods.
	/// </summary>
	public class NamedPipeIOException : InterProcessIOException
	{
		/// <summary>
		/// Creates a NamedPipeIOException instance.
		/// </summary>
		/// <param name="text">The error message text.</param>
		public NamedPipeIOException(String text) : base(text)
		{}

		/// <summary>
		/// Creates a NamedPipeIOException instance.
		/// </summary>
		/// <param name="text">The error message text.</param>
		/// <param name="errorCode">The native error code.</param>
		public NamedPipeIOException(String text, uint errorCode) : base(text)
		{
			ErrorCode = errorCode;
			if (errorCode == NamedPipeNative.ERROR_CANNOT_CONNECT_TO_PIPE)
				IsServerAvailable = false;
		}

		/// <summary>
		/// Creates a NamedPipeIOException instance.
		/// </summary>
		/// <param name="info">The serialization information.</param>
		/// <param name="context">The streaming context.</param>
		protected NamedPipeIOException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{}
	}
}