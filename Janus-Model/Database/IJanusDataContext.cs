using System.Data;

using LinqToDB;

namespace Rsdn.Janus
{
	public interface IJanusDataContext : IDataContext
	{
		IDbTransaction BeginTransaction();
	}
}