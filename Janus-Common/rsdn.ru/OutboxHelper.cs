using System;
using System.Linq;
using System.Linq.Expressions;

using CodeJam.Services;

using JetBrains.Annotations;

using LinqToDB;

namespace Rsdn.Janus
{
	public static class OutboxHelper
	{
		[NotNull]
		public static IOutboxManager GetOutboxManager(
			[NotNull] this IServiceProvider provider)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));
			return provider.GetRequiredService<IOutboxManager>();
		}

		#region Messages
		[NotNull]
		public static ITable<DataModel.IOutboxMessage> OutboxMessages([NotNull] this IDataContext db)
		{
			return db.GetTable<DataModel.IOutboxMessage>();
		}

		public static IQueryable<DataModel.IOutboxMessage> OutboxMessages(
			[NotNull] this IDataContext db,
			[NotNull] Expression<Func<DataModel.IOutboxMessage, bool>> predicate)
		{
			return db.GetTable(predicate);
		}

		public static void AddOutboxMessage(
			[NotNull] IServiceProvider provider,
			[NotNull] IOutboxMessage message)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));
			if (message == null) throw new ArgumentNullException(nameof(message));

			var tlm = provider.GetRequiredService<ITagLineManager>();
			var tagline = tlm.GetTagLine(tlm.FindAppropriateTagLine(message.ForumId));

			using (var db = provider.CreateDBContext())
				db
					.OutboxMessages()
						.Value(_ => _.ForumID, () => message.ForumId)
						.Value(_ => _.ReplyToID, () => message.ReplyId)
						.Value(_ => _.Date, () => DateTime.Now)
						.Value(_ => _.Subject, () => message.Subject)
						.Value(_ => _.Body, () => message.Message)
						.Value(_ => _.Hold, () => message.Hold)
						.Value(_ => _.Tagline, () => tagline)
						.Value(_ => _.Tags, () => message.Tags)
					.Insert();
		}
		#endregion

		#region Rates
		[NotNull]
		public static ITable<DataModel.IOutboxRate> OutboxRates(
			[NotNull] this IDataContext db)
		{
			return db.GetTable<DataModel.IOutboxRate>();
		}

		[NotNull]
		public static IQueryable<DataModel.IOutboxRate> OutboxRates(
			[NotNull] this IDataContext db,
			Expression<Func<DataModel.IOutboxRate, bool>> predicate)
		{
			return db.GetTable(predicate);
		}
		#endregion

		#region Download topics
		[NotNull]
		public static ITable<DataModel.IDownloadTopic> DownloadTopics([NotNull] this IDataContext db)
		{
			return db.GetTable<DataModel.IDownloadTopic>();
		}

		[NotNull]
		public static IQueryable<DataModel.IDownloadTopic> DownloadTopics(
			[NotNull] this IDataContext db,
			Expression<Func<DataModel.IDownloadTopic, bool>> predicate)
		{
			return db.GetTable(predicate);
		}
		#endregion
	}
}