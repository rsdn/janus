using System;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Data.Linq;

using JetBrains.Annotations;

using Rsdn.Janus;

namespace Janus.Rsdn.Janus
{
	public static class ViolationHelper
	{
		public static Table<IViolation> Violations([NotNull] this IDataContext db)
		{
			return db.GetTable<IViolation>();
		}

		public static IQueryable<IViolation> Violations(
			[NotNull] this IDataContext db,
			Expression<Func<IViolation, bool>> predicate)
		{
			return db.GetTable(predicate);
		}
	}
}