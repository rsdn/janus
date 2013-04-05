using System;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("violations")]
	public interface IViolation
	{
		int MessageID { get; }
		string Reason { get; }
		PenaltyType PenaltyType { get; }
		DateTime Create { get; }
	}
}