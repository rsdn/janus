using System;
using System.Runtime.Serialization;

using CodeJam;

using Rsdn.Janus.Database.Schema;

namespace Rsdn.Janus
{
	[Serializable]
	public class SchemaChangeException : ApplicationException
	{
		public SchemaChangeException(string cmdText, Exception innerException)
			: base(GetMessage(cmdText, innerException.Message), innerException)
		{}

		private static string GetMessage(string cmdText, string errorMessage)
		{
			return SchemaResources.SchemaExceptionMessage.FormatWith(cmdText, errorMessage);
		}

		protected SchemaChangeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{}
	}
}
