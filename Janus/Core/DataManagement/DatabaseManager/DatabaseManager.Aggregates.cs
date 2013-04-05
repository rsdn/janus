using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

using JetBrains.Annotations;

using LinqToDB;

using Rsdn.Janus.DataModel;

namespace Rsdn.Janus
{
	static partial class DatabaseManager
	{
		#region Агрегация данных в локальной БД
		/// <summary>
		/// Обновление агрегатов
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="db"></param>
		/// <param name="tidList">Массив Topic ID для препарации</param>
		public static void UpdateAggregates(
			IServiceProvider provider,
			IDataContext db,
			IEnumerable<int> tidList)
		{
			foreach (var series in tidList.SplitForInClause(provider))
			{
				var locSeries = series;
				UpdateTopicInfoByFilter(
					db,
					ti => locSeries.Contains(ti.MessageID),
					m => locSeries.Contains(m.ID));
			}
		}

		/// <summary>
		/// Обновляет агрегатные данные по темам (таблицу topic_info).
		/// Эта процедура сначала удаляет записи соответствующие параметру filter,
		/// а потом формирует агрегатные данные и добавляет их в topic_info.
		/// Транзакция должна контролироваться вызывающим методом.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="msgPredicate">Необязательный параметр позволяющий задать
		/// фильтрацию тем данные по которым нужно пересчитать. 
		/// Может быть null или string.Empty. Если параметр не задан, 
		/// обновляются все сообщения. При этом время обновления может быть
		/// довольно большим (на Athlon 2100+ 512 MB RAM ~ 1 мин.).
		/// Пример: "mid IN (1, 2, 3)" обновит агрегатные значения
		/// для тем 1, 2 и 3.</param>
		/// <param name="topicPredicate">комплиментарный <see cref="msgPredicate"/> предикат для TopicInfo</param>
		/// <returns>Возвращает количество вставленных записей.</returns>
		private static int UpdateTopicInfoByFilter(
			IDataContext db,
			[CanBeNull] Expression<Func<ITopicInfo, bool>> topicPredicate,
			[CanBeNull] Expression<Func<IForumMessage, bool>> msgPredicate)
		{
			// Удаляем.
			//
			if (topicPredicate != null)
				db
					.TopicInfos(topicPredicate)
					.Delete();
			else
				db.TopicInfos().Delete();

			var rates = db.Rates();
			var mods = db.Moderatorials();

			var msgs = db.Messages(m => m.TopicID == 0);
			if (msgPredicate != null)
				msgs = msgs.Where(msgPredicate);

			var updated =
				msgs
					.Into(db.TopicInfos())
						.Value(_ => _.MessageID, m => m.ID)
						.Value(_ => _.ForumID, m => m.ForumID)
						.Value(_ => _.AnswersCount, m => m.TopicAnswers.Count())
						.Value(
							_ => _.AnswersRates,
							m =>
								rates
									.Where(r => r.Message.TopicID == m.ID && r.RateType > 0)
									.Sum(r => (int)r.RateType * r.Multiplier))
						.Value(
							_ => _.AnswersUnread,
							m => m.TopicAnswers.Count(im => !im.IsRead) + (m.IsRead ? 0 : 1))
						.Value(
							_ => _.AnswersSmiles,
							m =>
								rates
									.Count(r => r.Message.TopicID == m.ID && r.RateType == MessageRates.Smile))
						.Value(
							_ => _.AnswersAgrees,
							m =>
								rates
									.Count(r => r.Message.TopicID == m.ID && r.RateType == MessageRates.Agree))
						.Value(
							_ => _.AnswersDisagrees,
							m =>
								rates
									.Count(r => r.Message.TopicID == m.ID && r.RateType == MessageRates.DisAgree))
						.Value(
							_ => _.AnswersToMeUnread,
							m =>
								m
									.TopicAnswers
									.Count(im => !im.IsRead && im.Parent.UserID == Config.Instance.SelfId))
						.Value(_ => _.AnswersMarked, m => m.TopicAnswers.Count(im => im.IsMarked))
						.Value(_ => _.LastUpdateDate, m => m.TopicAnswers.Max(im => (DateTime?)im.Date) ?? m.Date)
						.Value(
							_ => _.AnswersModeratorials,
							m =>
								mods
									.Count(
										mod =>
											mod.Message.TopicID == m.ID
											&& (mod.Message.LastModerated == null || mod.Create > mod.Message.LastModerated)))
						.Value(_ => _.SelfRates, m => m.Rating())
						.Value(_ => _.SelfSmiles, m => m.SmileCount())
						.Value(_ => _.SelfAgrees, m => m.AgreeCount())
						.Value(_ => _.SelfDisagrees, m => m.DisagreeCount())
						.Value(_ => _.SelfModeratorials, m => m.ActiveModeratorialCount())
					.Insert();

			return updated;
		}

		private static void UpdateTopicInfo(IDataContext db, int tid)
		{
			UpdateTopicInfoByFilter(
				db,
				ti => ti.MessageID == tid,
				m => m.ID == tid);
		}

		public static void UpdateTopicInfoRange(
			IServiceProvider provider,
			IDataContext db,
			IEnumerable<int> tids)
		{
			if (db == null)
				throw new ArgumentNullException("db");
			if (tids == null)
				return;

			foreach (var series in
					tids
						.SplitToSeries(provider.MaxInClauseElements()))
			{
				var locSeries = series;
				UpdateTopicInfoByFilter(
					db,
					ti => locSeries.Contains(ti.MessageID),
					m => locSeries.Contains(m.ID));
			}
		}

		public static void UpdateTopicInfoSpecified(
			IServiceProvider provider,
			IDataContext db,
			IEnumerable<int> affectedIds)
		{
			foreach (var portion in affectedIds.SplitForInClause(provider))
			{
				var locPortion = portion;

				db
					.TopicInfos(ti => locPortion.Contains(ti.MessageID))
						.Set(_ => _.AnswersUnread,     0)
						.Set(_ => _.AnswersToMeUnread, 0)
					.Update();

				var msgs = db.Messages();
				var affectedTids =
					msgs
						.Where(m => locPortion.Contains(m.ID))
						.Select(m => m.TopicID == 0 ? m.ID : m.TopicID)
						.Distinct();

				var readCounts =
					affectedTids
						.Select(
							tid =>
								new
								{
									TopicID = tid,
									UnreadCount =
										msgs.Count(m => !m.IsRead && m.TopicID == tid),
									MeUnreadCount =
										msgs.Count(m => !m.IsRead && m.UserID == Config.Instance.SelfId && m.TopicID == tid)
								})
						.ToList();

				foreach (var cnt in readCounts)
				{
					var locCnt = cnt;
					db
						.TopicInfos(ti => ti.MessageID == locCnt.TopicID)
							.Set(_ => _.AnswersUnread,     locCnt.UnreadCount)
							.Set(_ => _.AnswersToMeUnread, locCnt.MeUnreadCount)
						.Update();
				}
			}
		}

		public static void InitDbAggr(
			IServiceProvider provider,
			bool showGui,
			string msg)
		{
			if (showGui
					&& MessageBox.Show(
							string.Format(
								(!string.IsNullOrEmpty(msg) ? msg + "\n" : string.Empty)
									+ SR.Database.WarningBeforeAggregateRestructure,
								ApplicationInfo.ApplicationName),
							ApplicationInfo.ApplicationName,
							MessageBoxButtons.OKCancel,
							MessageBoxIcon.Information) == DialogResult.Cancel)
				return;
			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				var sw = Stopwatch.StartNew();

				//db.TopicInfos().Delete();
				var recordsProcessed = UpdateTopicInfoByFilter(db, null, null);
				tx.Commit();

				if (showGui)
					MessageBox.Show(
						string.Format(
							SR.Database.WarningAfterAggregateRestructure,
							sw.Elapsed.TotalSeconds,
							recordsProcessed,
							0),
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		#endregion

		#region Работа с многозапросными транзакциями
		public static void ClearTopicInfo(IServiceProvider provider)
		{
			using (var db = provider.CreateDBContext())
				db
					.TopicInfos()
					.Delete();
		}

		public static void CheckTopicInfoIntegrity(IServiceProvider provider)
		{
			using (var db = provider.CreateDBContext())
			{
				if (db.TopicInfos().Any())
					return;
				if (!db.Messages().Any())
					return;
			}

			// Таблица topic_info пустая, а messages не пустая.
			// Нужно обновить корневые метки.
			//
			InitDbAggr(provider, true, SR.Database.NeedRestructure2);
		}
		#endregion
	}
}
