using System;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Data.Linq;

using JetBrains.Annotations;

using Rsdn.Janus.DataModel;

namespace Rsdn.Janus
{
	public static class ForumDBHelper
	{
		#region ForumMessage helpers
		public static Table<IForumMessage> Messages([NotNull] this IDataContext db)
		{
			return db.GetTable<IForumMessage>();
		}

		public static IQueryable<IForumMessage> Messages(
			[NotNull] this IDataContext db,
			Expression<Func<IForumMessage, bool>> predicate)
		{
			return db.GetTable(predicate);
		}

		public static T Message<T>(
			[NotNull] this IDataContext db,
			int mid,
			Expression<Func<IForumMessage, T>> selector)
		{
			if (db == null) throw new ArgumentNullException("db");
			return
				db
					.Messages(m => m.ID == mid)
					.Select(selector)
					.FirstOrDefault();
		}

		public static Table<ITopicInfo> TopicInfos([NotNull] this IDataContext db)
		{
			return db.GetTable<ITopicInfo>();
		}

		public static IQueryable<ITopicInfo> TopicInfos(
			[NotNull] this IDataContext db,
			[NotNull] Expression<Func<ITopicInfo, bool>> predicate)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");
			return db.GetTable(predicate);
		}

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
		#endregion

		#region Forum helpers
		public static Table<IServerForum> ServerForums([NotNull] this IDataContext db)
		{
			return db.GetTable<IServerForum>();
		}

		public static IQueryable<IServerForum> ServerForums(
			[NotNull] this IDataContext db,
			Expression<Func<IServerForum, bool>> predicate)
		{
			return db.GetTable(predicate);
		}

		public static Table<ISubscribedForum> SubscribedForums([NotNull] this IDataContext db)
		{
			return db.GetTable<ISubscribedForum>();
		}

		public static IQueryable<ISubscribedForum> SubscribedForums(
			[NotNull] this IDataContext db,
			Expression<Func<ISubscribedForum, bool>> predicate)
		{
			return db.GetTable(predicate);
		}
		#endregion

		#region Moderatorial helpers
		public static Table<IModeratorial> Moderatorials([NotNull] this IDataContext db)
		{
			return db.GetTable<IModeratorial>();
		}

		public static IQueryable<IModeratorial> Moderatorials(
			[NotNull] this IDataContext db,
			Expression<Func<IModeratorial, bool>> predicate)
		{
			return db.GetTable(predicate);
		}

		[MethodExpression("ActiveModeratorialCountExpression")]
		public static int ActiveModeratorialCount(this IForumMessage message)
		{
			throw new NotSupportedException();
		}

		[UsedImplicitly]
		private static Expression ActiveModeratorialCountExpression()
		{
			return
				(Expression<Func<IForumMessage, int>>)
				(message =>
					Sql.AsSql(
						message.LastModerated == null
							? message.Moderatorials.Count()
							: message.Moderatorials.Count(
								mod => mod.Create > message.LastModerated)));
		}
		#endregion

		#region Rate helpers
		public static Table<IRate> Rates([NotNull] this IDataContext db)
		{
			return db.GetTable<IRate>();
		}

		public static IQueryable<IRate> Rates(
			[NotNull] this IDataContext db,
			Expression<Func<IRate, bool>> predicate)
		{
			return db.GetTable(predicate);
		}

		[MethodExpression("RatingExpression")]
		public static int Rating(this IForumMessage message)
		{
			throw new NotSupportedException();
		}

		[UsedImplicitly]
		private static Expression RatingExpression()
		{
			return
				(Expression<Func<IForumMessage, int>>)
				(m =>
					m
						.Rates
						.Where(r => r.RateType > 0)
						.Sum(r => (short)r.RateType * r.Multiplier));
		}

		[MethodExpression("SmileCountExpression")]
		public static int SmileCount(this IForumMessage message)
		{
			throw new NotSupportedException();
		}

		[UsedImplicitly]
		private static Expression SmileCountExpression()
		{
			return
				(Expression<Func<IForumMessage, int>>)
				(m => m.Rates.Count(r => r.RateType == MessageRates.Smile));
		}

		[MethodExpression("AgreeCountExpression")]
		public static int AgreeCount(this IForumMessage message)
		{
			throw new NotSupportedException();
		}

		[UsedImplicitly]
		private static Expression AgreeCountExpression()
		{
			return
				(Expression<Func<IForumMessage, int>>)
				(m => m.Rates.Count(r => r.RateType == MessageRates.Agree));
		}

		[MethodExpression("DisagreeCountExpression")]
		public static int DisagreeCount(this IForumMessage message)
		{
			throw new NotSupportedException();
		}

		[UsedImplicitly]
		private static Expression DisagreeCountExpression()
		{
			return
				(Expression<Func<IForumMessage, int>>)
				(m => m.Rates.Count(r => r.RateType == MessageRates.DisAgree));
		}
		#endregion
	}
}