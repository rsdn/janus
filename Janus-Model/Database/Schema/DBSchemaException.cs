using System;
using System.Runtime.Serialization;

namespace Rsdn.Janus
{
	[Serializable]
	public class DBSchemaException : Exception
	{
		public DBSchemaException()
		{}

		public DBSchemaException(string message)
			: base(message)
		{}

		public DBSchemaException(string message, Exception innerException)
			: base(message, innerException)
		{}

		protected DBSchemaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{}
	}
}