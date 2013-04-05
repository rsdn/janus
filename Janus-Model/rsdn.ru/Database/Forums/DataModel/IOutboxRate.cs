using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("rate_outbox")]
	public interface IOutboxRate
	{
		[Column] 
		int ID { get; }

		[Column("mid")]
		int MessageID { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Message { get; }

		[Column("rate")]
		MessageRates RateType { get; }
	}
}