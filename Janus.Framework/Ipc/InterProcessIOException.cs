using System;
using System.Runtime.Serialization;

namespace Rsdn.Janus.Framework.Ipc
{
	public class InterProcessIOException : Exception
	{
		public uint ErrorCode;
		public bool IsServerAvailable = true;

		public InterProcessIOException(String text) : base(text)
		{}

		protected InterProcessIOException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{}
	}
}