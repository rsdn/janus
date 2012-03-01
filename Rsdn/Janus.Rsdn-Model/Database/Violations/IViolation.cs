using System;

using BLToolkit.DataAccess;

namespace Janus.Rsdn.Janus
{
	[TableName("violations")]
	public interface IViolation
	{
		int MessageID { get; }
		string Reason { get; }
		PenaltyType PenaltyType { get; }
		DateTime Create { get; }
	}
}