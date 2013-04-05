using System;
using System.Linq;
using System.Linq.Expressions;

using JetBrains.Annotations;

using LinqToDB;
using LinqToDB.Linq;

using Rsdn.Janus.DataModel;

namespace Rsdn.Janus
{
	public static class FavoritesHelper
	{
		public static Table<IFavoritesFolder> FavoriteFolders([NotNull] this IDataContext db)
		{
			if (db == null) throw new ArgumentNullException("db");
			return db.GetTable<IFavoritesFolder>();
		}

		public static IQueryable<IFavoritesFolder> FavoriteFolders(
			[NotNull] this IDataContext db,
			[CanBeNull] Expression<Func<IFavoritesFolder, bool>> predicate)
		{
			return db.GetTable(predicate);
		}

		public static IValueInsertable<IFavoritesFolder> IntoFavoriteFolders([NotNull] this IDataContext db)
		{
			if (db == null) throw new ArgumentNullException("db");
			return db.Into(db.FavoriteFolders());
		}

		public static Table<IFavoritesItem> FavoriteItems([NotNull] this IDataContext dbMgr)
		{
			if (dbMgr == null) throw new ArgumentNullException("dbMgr");
			return dbMgr.GetTable<IFavoritesItem>();
		}

		public static IQueryable<IFavoritesItem> FavoriteItems(
			[NotNull] this IDataContext dbMgr,
			[CanBeNull] Expression<Func<IFavoritesItem, bool>> predicate)
		{
			return dbMgr.GetTable(predicate);
		}

		public static IValueInsertable<IFavoritesItem> IntoFavoriteItems(
			[NotNull] this IDataContext db)
		{
			if (db == null) throw new ArgumentNullException("db");
			return db.Into(db.FavoriteItems());
		}
	}
}