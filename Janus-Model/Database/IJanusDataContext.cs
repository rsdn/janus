using System.Data;

using BLToolkit.Data.Linq;

namespace Rsdn.Janus
{
	public interface IJanusDataContext : IDataContext
	{
		IDbTransaction BeginTransaction();
	}
}