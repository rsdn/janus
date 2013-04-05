using System;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("rating")]
	public interface IRate
	{
		int ID { get; }

		[Column("dte")]
		DateTime Date { get; }

		[Column("rate")]
		MessageRates RateType { get; }

		[Column("rby")]
		short Multiplier { get; }

		[Column("mid")]
		int MessageID { get; }

		[Column("tid")]
		int TopicID { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = false)]
		IForumMessage Message { get; }

		[Column("uid")]
		int UserID { get; }

		[Association(ThisKey = "UserID", OtherKey = "ID", CanBeNull = true)]
		IUser User { get; }
	}
}