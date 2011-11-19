using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("rate_outbox")]
	public interface IOutboxRate
	{
		int ID { get; }

		[MapField("mid")]
		int MessageID { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Message { get; }

		[MapField("rate")]
		MessageRates RateType { get; }
	}
}