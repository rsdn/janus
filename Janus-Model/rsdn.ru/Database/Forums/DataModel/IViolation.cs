using System;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("violations")]
	public interface IViolation
	{
		[Column] int MessageID { get; }
		[Column] string Reason { get; }
		[Column] PenaltyType PenaltyType { get; }
		[Column] DateTime Create { get; }
	}
}