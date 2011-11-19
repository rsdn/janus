using System;
using System.Runtime.Serialization;

using Rsdn.Janus.Database.Schema;
using Rsdn.SmartApp;

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
			return SchemaResources.SchemaExceptionMessage.FormatStr(cmdText, errorMessage);
		}

		protected SchemaChangeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{}
	}
}
