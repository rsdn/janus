using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

using Lucene.Net.Analysis.Ru;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

using Rsdn.Janus.Framework;
using Rsdn.Janus.Log;

using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace Rsdn.Janus
{
	internal static class SearchHelper
	{
		private const int _maxSearchReults = 10000;

		private const string _signature = "**";

		/// <summary>
		/// Форматирование даты сообщения для использования при построении поискового индекса
		/// </summary>
		private static string FormatDate(DateTime date)
		{
			return date.ToString("yyyyMMdd");
		}

		private static Directory GetIndexDir()
		{
			return FSDirectory.Open(new DirectoryInfo(LocalUser.LuceneIndexPath));
		}

		/// <summary>
		/// Получение класса для построения индекса, оптимизированного под обработку больших объёмов данных
		/// </summary>
		public static IndexWriter CreateIndexWriter()
		{
			var indexPath = GetIndexDir();
			var w =
				new IndexWriter(
					indexPath,
						new RussianAnalyzer(),
						!IndexReader.IndexExists(indexPath),
						IndexWriter.MaxFieldLength.UNLIMITED);
			// optimizing
			w.SetMaxBufferedDocs(4*1024);
			//w.SetMergeFactor(30); 
			return w;
		}

		/// <summary>
		/// Построение поискового индекса по полученным в результате синхронизации сообщениям
		/// </summary>
		public static int ProcessResponseMessages(IEnumerable<MessageSearchInfo> messages)
		{
			if (messages == null)
				throw new ArgumentNullException("messages");

			int addedCount;

			var indexPath = GetIndexDir();

			// Чистим
			if (IndexReader.IndexExists(indexPath))
			{
				var reader = IndexReader.Open(indexPath, false);
				var baseTerm = new Term("mid");

				try
				{
					foreach (var msg in messages)
					{
						var term = baseTerm.CreateTerm(msg.MessageBody ?? "");
						reader.DeleteDocuments(term);
					}
				}
				finally
				{
					reader.Close();
				}
			}

			//Добавляем
			var writer = CreateIndexWriter();
			try
			{
				var count = 0;
				foreach (var msg in messages)
				{
					// Форумы с id 0 и 58 это мусорки
					if (msg.ForumID == 0 || msg.ForumID == 58)
						continue;
					writer.AddDocument(
						CreateDocument(
							msg.MessageID.ToString(),
							msg.ForumID.ToString(),
							FormatDate(msg.MessageDate),
							msg.Subject,
							msg.UserID.ToString(),
							msg.UserNick,
							msg.MessageBody));
					count++;
				}
				addedCount = count;
			}
			finally
			{
				writer.Close();
			}

			return addedCount;
		}

		/// <summary>
		/// Поиск на основе ранее построенного индекса
		/// </summary>
		private static ICollection<string> Search(
			int forumID,
			string searchText,
			bool searchInText,
			bool searchInSubject,
			bool searchAuthor,
			bool searchInMyMessages,
			bool searchAnyWords,
			DateTime from,
			DateTime to)
		{
			var result = new List<string>();
			var query = new BooleanQuery();
			var analyzer = new RussianAnalyzer();
			var indexPath = GetIndexDir();
			var searchTextExists = !string.IsNullOrEmpty(searchText);

			#region Обработка строки
			// Сигнатура языка поиска - **
			if (searchTextExists)
			{
				if (searchText.StartsWith(_signature))
				{
					// Да, хотим использовать язык, отрезаем ** и считаем остаток строки написанным на языке поиска
					searchText = searchText.Substring(_signature.Length);
				}
				else
				{
					// Используем простой поиск: экранируем спецсимволы, получаем токены (пробел - разделитель), учитываем флажок searchAnyWords (AND/OR)
					// Порядок важен, первое - \\
					var specChars = new[] {"\\", "+", "-", "&", "|", "!", "(", ")", "{", "}", "[", "]", "^", "\"", "~", "*", "?", ":"};
					searchText =
						specChars
							.Aggregate(
								searchText,
								(current, specChar) => current.Replace(specChar, "\\" + specChar));
					var token = searchText.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

					if (searchAnyWords)
						searchText = string.Join(" ", token);
					else
						searchText = "+" + string.Join(" +", token);
				}
			}
			#endregion

			if (forumID != -1)
				query.Add(
					new TermQuery(new Term("gid", forumID.ToString())),
					BooleanClause.Occur.MUST);

			if (searchInMyMessages)
				query.Add(
					new TermQuery(new Term("uid", Config.Instance.SelfId.ToString())),
					BooleanClause.Occur.MUST);

			//if (searchInQuestions)
			//  bq.Add(new TermQuery(new Term("tid", "0")), true, false);

			if (from.Ticks != 0 || to.Ticks != 0)
			{
				var rq = new TermRangeQuery("dte", FormatDate(from), FormatDate(to), true, true);
				query.Add(rq, BooleanClause.Occur.MUST);
			}
			
			if (searchTextExists)
			{
				var searchTextQuery = new BooleanQuery();
				if (searchInText)
					searchTextQuery.Add(
						new QueryParser(Version.LUCENE_CURRENT, "message", analyzer).Parse(searchText),
						BooleanClause.Occur.SHOULD);
				if (searchInSubject)
					searchTextQuery.Add(
						new QueryParser(Version.LUCENE_CURRENT, "subject", analyzer).Parse(searchText),
						BooleanClause.Occur.SHOULD);
				if (searchAuthor)
					searchTextQuery.Add(
						new QueryParser(Version.LUCENE_CURRENT, "usernick", analyzer).Parse(searchText),
						BooleanClause.Occur.SHOULD);
				query.Add(searchTextQuery, BooleanClause.Occur.MUST);
			}

			var searcher = new IndexSearcher(indexPath, true);
			try
			{
				var topDocs = searcher.Search(query, _maxSearchReults);
				result
					.AddRange(
						topDocs
							.scoreDocs
							.Select(scored => searcher.Doc(scored.doc).Get("mid")));
			}
			finally
			{
				searcher.Close();
			}

			return result;
		}

		/// <summary>
		/// Создание документа в формате Lucene для последующей индексации
		/// </summary>
		private static Document CreateDocument(
			string mid,
			string gid,
			string dte,
			string subject,
			string uid,
			string usernick,
			string message)
		{
			var doc = new Document();

			doc.Add(new Field("mid", mid, Field.Store.YES, Field.Index.NOT_ANALYZED)); // Созраняем в индексе только mid
			doc.Add(new Field("gid", gid, Field.Store.NO, Field.Index.NOT_ANALYZED_NO_NORMS));
			doc.Add(new Field("dte", dte, Field.Store.NO, Field.Index.NOT_ANALYZED_NO_NORMS));
			doc.Add(new Field("subject", subject, Field.Store.NO, Field.Index.ANALYZED));
			doc.Add(new Field("uid", uid, Field.Store.NO, Field.Index.NOT_ANALYZED_NO_NORMS));
			doc.Add(new Field("usernick", usernick, Field.Store.NO, Field.Index.ANALYZED));
			doc.Add(new Field("message", message, Field.Store.NO, Field.Index.ANALYZED));

			return doc;
		}

		public static Document CreateDocument([NotNull] this MessageSearchInfo info)
		{
			if (info == null) throw new ArgumentNullException("info");
			return
				CreateDocument(
					info.MessageID.ToString(),
					info.ForumID.ToString(),
					FormatDate(info.MessageDate),
					info.Subject,
					info.UserID.ToString(),
					info.UserNick,
					info.MessageBody);
		}

		/// <summary>
		/// Поиск и выборка данных с использованием Lucene
		/// </summary>
		public static List<MsgBase> SearchMessagesByLucene(
			IServiceProvider provider,
			int forumId,
			string searchText,
			bool searchInText,
			bool searchInSubject,
			bool searchAuthor,
			bool searchInMarked,
			bool searchInMyMessages,
			bool searchAnyWords,
			bool searchInQuestions,
			DateTime from,
			DateTime to)
		{
			provider.LogInfo("Начат поиск...");
			var maxInClause = provider.MaxInClauseElements();

			provider.LogInfo("Используется lucene...");

			var mids =
				Search(
					forumId,
					searchText,
					searchInText,
					searchInSubject,
					searchAuthor,
					//searchInMarked,
					searchInMyMessages,
					searchAnyWords,
					//searchInQuestions,
					from,
					to);

			if (mids.Count == 0)
				return new List<MsgBase>(); // Ничего не нашли, вернём пустой список

			if (mids.Count > maxInClause)
				mids = Algorithms.GetFirstElements(mids, maxInClause);
			var intMids = mids.Select(mid => int.Parse(mid));

			using (var db = provider.CreateDBContext())
			{
				var q = db.Messages(m => intMids.Contains(m.ID));
				q = q.OrderByDescending(m => m.ID);

				// Обработка вопросов
				if (searchInQuestions)
					q = q.Where(m => m.TopicID == 0);

				// Обработка пометок пользователя
				if (searchInMarked)
					q = q.Where(m => m.IsMarked);

				var maxResults = Config.Instance.SearchConfig.MaxResultInSelect;
				if (maxResults > 0)
					q = q.Take(maxResults);

				var msgTable = db.Messages();
				var msgs =
					q
						.Select(
							m =>
								new LinearTreeMsg(provider)
								{
									ID = m.ID,
									ForumID = m.ForumID,
									TopicIDInternal = m.TopicID,
									ParentID = m.ParentID,
									Date = m.Date,
									UserID = m.UserID,
									UserNick = m.UserNick,
									UserClass = (short)m.UserClass,
									Subject = m.Subject,
									IsRead = m.IsRead,
									Rating = m.Rating(),
									Smiles = m.SmileCount(),
									Agrees = m.AgreeCount(),
									Disagrees = m.DisagreeCount(),
									RepliesCount = msgTable.Count(im => im.ParentID == m.ID),
								});
				var list =
					msgs
						.Cast<MsgBase>()
						.ToList();
				foreach (var msg in list)
					msg.EndMapping(null);
				return list;
			}
		}
	}
}