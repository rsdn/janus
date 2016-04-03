using System;
using System.Linq;
using System.Linq.Expressions;

using CodeJam;

using JetBrains.Annotations;

using LinqToDB;

using Rsdn.Janus.Database;
using Rsdn.Janus.DataModel;

namespace Rsdn.Janus
{
	public static class UserHelper
	{
		public static readonly string AnonymousDisplayName =
			DatabaseResources.AnonymousDisplayName;

		public static string ToUserDisplayName(this string userNick, UserClass userClass)
		{
			var name =
				userClass == UserClass.Anonym && userNick.IsNullOrEmpty()
					? AnonymousDisplayName
					: userNick;
			return name ?? "";
		}

		/// <summary>
		/// Возвращает первое не пустое значение, 
		/// иначе - Аноним
		/// </summary>
		/// <param name="nick">Псевдоним пользователя.</param>
		/// <param name="realName">Реальное имя пользователя.</param>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="userClass">класс пользователя</param>
		/// <returns>Первое не пустое "имя" пользователя, иначе - Аноним.</returns>
		public static string ToUserDisplayName(
			string nick,
			string realName,
			string userName,
			UserClass userClass)
		{
			if (!string.IsNullOrEmpty(nick)) return nick;
			if (!string.IsNullOrEmpty(realName)) return realName;
			if (!string.IsNullOrEmpty(userName)) return userName;

			return AnonymousDisplayName;
		}

		public static ITable<IUser> Users([NotNull] this IDataContext db)
		{
			if (db == null) throw new ArgumentNullException(nameof(db));
			return db.GetTable<IUser>();
		}

		public static IQueryable<IUser> Users(
			[NotNull] this IDataContext db,
			[CanBeNull] Expression<Func<IUser, bool>> predicate)
		{
			return db.GetTable(predicate);
		}

		[CanBeNull]
		public static T User<T>(
			[NotNull] this IDataContext db,
			int uid,
			[NotNull] Expression<Func<IUser, T>> selector)
		{
			if (db == null) throw new ArgumentNullException(nameof(db));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			return
				db
					.Users(u => u.ID == uid)
					.Select(selector)
					.FirstOrDefault();
		}

		[ExpressionMethod("DisplayNameExpression")]
		public static string DisplayName(this IUser user)
		{
			throw new NotSupportedException();
		}

		[UsedImplicitly]
		private static Expression<Func<IUser, string>> DisplayNameExpression()
		{
			return u => ToUserDisplayName(u.Nick, u.RealName, u.Name, u.UserClass);
		}
	}
}