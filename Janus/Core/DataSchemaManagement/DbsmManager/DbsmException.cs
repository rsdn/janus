using System;
using System.Runtime.Serialization;

namespace Rsdn.Janus
{
	[Serializable]
	public class DbsmException : Exception
	{
		public DbsmException()
		{
		}

		public DbsmException(string message)
			: base(message)
		{
		}

		public DbsmException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected DbsmException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

}
