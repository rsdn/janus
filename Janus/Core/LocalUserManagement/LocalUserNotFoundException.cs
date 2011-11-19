using System;
using System.Runtime.Serialization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Исключение
	/// </summary>
	[Serializable]
	public class LocalUserNotFoundException : Exception
	{
		public LocalUserNotFoundException()
		{
		}

		public LocalUserNotFoundException(string message)
			: base(message)
		{
		}

		public LocalUserNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected LocalUserNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}