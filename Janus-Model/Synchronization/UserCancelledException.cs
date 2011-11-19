using System;
using System.Runtime.Serialization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Исключение, вызываемое при отмене пользователем.
	/// </summary>
	[Serializable]
	public class UserCancelledException : Exception
	{
		public UserCancelledException() : base(SyncResources.UserCancel)
		{}

		protected UserCancelledException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{}
	}
}