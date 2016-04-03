using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using CodeJam.Extensibility;

using JetBrains.Annotations;

using LinqToDB;

namespace Rsdn.Janus
{
	public sealed class StateObject
	{
		#region stateBag
		[Serializable]
		[UsedImplicitly]
		public sealed class StateBag
		{
			#region Public Properties
			public int MaxMessageId { get; set; }

			public List<int> UnreadMessages { get; set; }

			public List<int> MarkedMessages { get; set; }

			public List<int> MarkedTopics { get; set; }

			public Folder Favorites { get; set; }
			#endregion
		}
		#endregion

		#region Favorites

		#region Link
		[Serializable]
		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		public sealed class Link
		{
			public Link()
			{ }

			public Link(int mid, string url, string comment)
			{
				MessageId = mid;
				Url = url;
				Comment = comment;
			}

			[UsedImplicitly]
			public int MessageId { get; set; }
			[UsedImplicitly]
			public string Url { get; set; }
			[UsedImplicitly]
			public string Comment { get; set; }
		}
		#endregion

		#region Folder
		[Serializable]
		[UsedImplicitly]
		public sealed class Folder
		{
			public Folder()
			{
				Links = new List<Link>();
				Folders = new List<Folder>();
			}

			public Folder(string name, string comment)
				: this()
			{
				Name = name;
				Comment = comment;
			}

			[UsedImplicitly]
			public string Name { get; set; }
			[UsedImplicitly]
			public string Comment { get; set; }
			[UsedImplicitly]
			public List<Link> Links { get; set; }
			[UsedImplicitly]
			public List<Folder> Folders { get; set; }
		}
		#endregion

		#endregion

		#region Constructor

		private readonly IServiceProvider _serviceProvider;
		private readonly IFavoritesManager _favManager;

		public StateObject([NotNull] IServiceProvider serviceProvider, [NotNull]string filename)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));

			_serviceProvider = serviceProvider;
			_favManager = serviceProvider.GetRequiredService<IFavoritesManager>();
			FileName = filename;
		}

		#endregion

		#region Publc Properties
		private string FileName { get; }
		#endregion

		#region Public Methods
		/// <summary>
		/// Сохраняет избранное, прочитанное и маркеры...
		/// </summary>
		/// <param name="options">Опции сохранения.</param>
		public void SaveState(SaveStateOptions options)
		{
			if (options == SaveStateOptions.None)
				return;

			using (var db = _serviceProvider.CreateDBContext())
			{
				var state = new StateBag
				{
					MaxMessageId = db.Messages().Max(msg => msg.ID)
				};

				if ((options & SaveStateOptions.ReadedMessages) != SaveStateOptions.None)
					state.UnreadMessages =
						db
							.Messages(m => !m.IsRead)
							.Select(m => m.ID)
							.ToList();

				if ((options & SaveStateOptions.Markers) != SaveStateOptions.None)
				{
					state.MarkedMessages =
						db
							.Messages(m => m.IsMarked)
							.Select(m => m.ID)
							.ToList();
					state.MarkedTopics =
						db
							.TopicInfos(ti => ti.AnswersMarked > 0)
							.Select(ti => ti.MessageID)
							.ToList();
				}

				if ((options & SaveStateOptions.Favorites) != SaveStateOptions.None)
					state.Favorites = GetFavorites(_favManager.RootFolder);

				Serialize(state);
			}
		}

		/// <summary>
		/// Восстанавливает избранное, прочитанное и маркеры...
		/// </summary>
		/// <param name="options">Опции восстановления.</param>
		public void RestoreState(RestoreStateOptions options)
		{
			const RestoreStateOptions noOptions = RestoreStateOptions.None;

			if (options == noOptions)
				return;

			var state = Deserialize();

			// Восстанавливаем пометки...
			if ((options & RestoreStateOptions.Markers) != noOptions)
			{
				var clear =
					(options & RestoreStateOptions.ClearMarkers) != noOptions;

				RestoreMarkers(
					_serviceProvider,
					state.MarkedMessages,
					state.MarkedTopics,
					state.MaxMessageId,
					clear);
			}

			// Восстанавливаем Избранное
			if ((options & RestoreStateOptions.Favorites) != noOptions)
			{
				if ((options & RestoreStateOptions.ClearFavorites) != noOptions)
					ClearFavorites();

				RestoreFavorites(
					_favManager.RootFolder,
					state.Favorites.Folders, state.Favorites.Links);
			}

			// Прочитанные сообщения
			if ((options & RestoreStateOptions.ReadedMessages) != noOptions)
				if (state.UnreadMessages != null)
					ForumHelper.MarkMessagesById(_serviceProvider, state.UnreadMessages, state.MaxMessageId);
		}
		#endregion

		#region Private Helpers

		#region Serialize & Deserialize
		private void Serialize(StateBag stateBag)
		{
			var serializer =
				new XmlSerializer(typeof(StateBag));

			using (var fs = File.Create(FileName))
				serializer.Serialize(fs, stateBag);
		}

		private StateBag Deserialize()
		{
			var serializer =
				new XmlSerializer(typeof(StateBag));

			using (var fs = File.OpenRead(FileName))
				return (StateBag)serializer.Deserialize(fs);
		}
		#endregion

		#region Работа с пометками

		private static void SetMarkers(
			IServiceProvider provider,
			IEnumerable<int> mids,
			IEnumerable<int> tids,
			int maxMsgId,
			bool clearOther)
		{
			if (mids == null)
				throw new ArgumentNullException(nameof(mids));

			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				if (clearOther)
					ClearMarkers(db, maxMsgId);

				foreach (var series in mids.SplitForInClause(provider))
				{
					var locSeries = series;
					db
						.Messages(m => locSeries.Contains(m.ID))
							.Set(_ => _.IsMarked, true)
						.Update();
				}
				DatabaseManager.UpdateTopicInfoRange(provider, db, tids);

				tx.Commit();
			}
		}

		/// <summary>
		/// Удаляет маркеры.
		/// </summary>
		/// <param name="db">См. <see cref="IDataContext"/></param>
		/// <param name="maxMsgId">Максимальный ID сообщения, 
		/// который учавствует в выборке.</param>
		private static void ClearMarkers(IDataContext db, int maxMsgId)
		{
			// Транзакцией занимается вызывающий код.
			db
				.Messages(m => m.IsMarked && m.ID <= maxMsgId)
					.Set(_ => _.IsMarked, false)
				.Update();

			db
				.TopicInfos(ti => ti.AnswersMarked > 0 && ti.MessageID <= maxMsgId)
					.Set(_ => _.AnswersMarked, 0)
				.Update();
		}

		private static void RestoreMarkers(
			IServiceProvider provider,
			IEnumerable<int> markedMessages,
			IEnumerable<int> markedTopics,
			int maxMsgId,
			bool clearOther)
		{
			if (markedMessages == null)
				return;

			SetMarkers(
				provider,
				markedMessages,
				markedTopics,
				maxMsgId,
				clearOther);
		}
		#endregion

		#region Работа с Избранным
		private void RestoreFavorites(FavoritesFolder favoritesFolder,
			IEnumerable<Folder> folders, IEnumerable<Link> links)
		{
			foreach (var link in links)
				if (link.MessageId != 0)
					_favManager.AddMessageLink(
						link.MessageId, link.Comment, favoritesFolder);
				else
					_favManager.AddUrlLink(
						link.Url, link.Comment, favoritesFolder);

			foreach (var folder in folders)
			{
				_favManager.AddFolder(folder.Name, folder.Comment, favoritesFolder);

				var parentFolder =
// ReSharper disable AccessToModifiedClosure
					favoritesFolder.SubFolders.FirstOrDefault(f => f.Name == folder.Name);
// ReSharper restore AccessToModifiedClosure

				if (parentFolder != null)
					RestoreFavorites(parentFolder, folder.Folders, folder.Links);
			}
		}

		private void ClearFavorites()
		{
			var rootFolder = _favManager.RootFolder;

			var folders = new ArrayList(rootFolder.SubFolders);
			var links = new ArrayList(rootFolder.Links);

			foreach (FavoritesFolder folder in folders)
				_favManager.Delete(folder);

			foreach (FavoritesLink link in links)
				_favManager.Delete(link);
		}

		private static Folder GetFavorites(FavoritesFolder favoritesFolder)
		{
			var folder = new Folder(
				favoritesFolder.Name, favoritesFolder.Comment);

			foreach (var subFolder in favoritesFolder.SubFolders)
				folder.Folders.Add(GetFavorites(subFolder));

			foreach (var link in favoritesFolder.Links)
				folder.Links.Add(
					new Link(link.MessageId, link.Link, link.Comment));

			return folder;
		}
		#endregion

		#endregion
	}
}