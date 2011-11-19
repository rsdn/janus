using System;
using System.Runtime.Serialization;

namespace Rsdn.Janus
{
	[Serializable]
	public class OnlyForQueriesException : Exception
	{
		public OnlyForQueriesException()
			: base("This member can be used only in queries.")
		{}

		protected OnlyForQueriesException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{}
	}
}