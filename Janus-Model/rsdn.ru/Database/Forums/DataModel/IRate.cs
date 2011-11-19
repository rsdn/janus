using System;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("rating")]
	public interface IRate
	{
		int ID { get; }

		[MapField("dte")]
		DateTime Date { get; }

		[MapField("rate")]
		MessageRates RateType { get; }

		[MapField("rby")]
		short Multiplier { get; }

		[MapField("mid")]
		int MessageID { get; }

		[MapField("tid")]
		int TopicID { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = false)]
		IForumMessage Message { get; }

		[MapField("uid")]
		int UserID { get; }

		[Association(ThisKey = "UserID", OtherKey = "ID", CanBeNull = true)]
		IUser User { get; }
	}
}