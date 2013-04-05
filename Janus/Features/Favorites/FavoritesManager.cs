using System;
using System.Collections.Generic;
using System.Linq;

using LinqToDB;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public enum SortDirection
	{
		Asc,
		Desc
	}

	/// <summary>
	/// Класс, управляющий Избранным
	/// </summary>
	[Service(typeof (IFavoritesManager))]
	internal class FavoritesManager : IFavoritesManager
	{
		private readonly IServiceProvider _provider;

		public FavoritesFolder RootFolder { get; private set; }

		public event EventHandler FavoritesReloaded;

		private Dictionary<int, FavoritesFolder> GetFavorites()
		{
			using (var db = _provider.CreateDBContext())
			{
				var folders =
					db
						.FavoriteFolders()
						.Select(
							ff =>
								new FavoritesFolder(_provider)
								{
									Id = ff.ID,
									Name = ff.Name,
									ParentId = ff.ParentID,
									Comment = ff.Comment
								})
						.ToDictionary(ff => ff.Id);

				var links =
					db
						.FavoriteItems()
						.Select(
							i =>
								new FavoritesLink(_provider)
								{
									Id = i.ID,
									FolderId = i.FolderID,
									Comment = i.Comment,
									Url = i.Url,
									MessageId = i.MessageID,
								})
						.ToList();

				var rootFolder =
					FavoritesFolder.CreateInstance(
						_provider,
						0,
						string.Empty,
						string.Empty,
						null);

				folders.Add(0, rootFolder);

				foreach (var link in links)
				{
					var folder = folders[link.FolderId];

					folder.Links.Add(link);
					link.Parent = folder;
				}

				return folders;
			}
		}

		public void Reload()
		{
			RootFolder = CreateFavoritesFolders(GetFavorites());

			OnReload();
			SetFavoriteLinksSetDirty();
		}

		public void AddFolder(string name, string comment, FavoritesFolder parentFolder)
		{
			if (parentFolder.SubFolders.Any(f => f.Name == name))
				return;

			var newFolder =
				FavoritesFolder.CreateInstance(_provider, -1, name, comment, parentFolder);

			using (var db = _provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				db
					.IntoFavoriteFolders()
						.Value(_ => _.Name,     name)
						.Value(_ => _.ParentID, parentFolder.Id)
						.Value(_ => _.Comment,  comment)
					.Insert();

				newFolder.Id = db.FavoriteFolders().Max(ff => ff.ID);
				tx.Commit();
			}
			parentFolder.SubFolders.Add(newFolder);
			SetFavoriteLinksSetDirty();
		}

		private void InsertFavoritesLink(FavoritesLink link)
		{
			using (var db = _provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				db
					.IntoFavoriteItems()
						.Value(_ => _.MessageID, link.MessageId)
						.Value(_ => _.FolderID,  link.Parent.Id)
						.Value(_ => _.Comment,   link.Comment)
						.Value(_ => _.Url,       link.Url)
					.Insert();

				link.Id = db.FavoriteItems().Max(fi => fi.ID);

				tx.Commit();
			}
		}

		public bool AddMessageLink(int messageId, string comment, FavoritesFolder folder)
		{
			if (FindByMessageId(folder.Links, messageId) != null)
				return false;

			var link =
				FavoritesLink.CreateInstance(_provider, -1, messageId, comment, folder);

			folder.Links.Add(link);
			InsertFavoritesLink(link);
			SetFavoriteLinksSetDirty();
			return true;
		}

		public void AddUrlLink(string url, string comment, FavoritesFolder folder)
		{
			var link =
				FavoritesLink.CreateInstance(_provider, -1, url, comment, folder);

			folder.Links.Add(link);
			InsertFavoritesLink(link);
			SetFavoriteLinksSetDirty();
		}

		public void Update(FavoritesFolder folder)
		{
			using (var db = _provider.CreateDBContext())
				db
					.FavoriteFolders(ff => ff.ID == folder.Id)
						.Set(_ => _.Name, folder.Name)
						.Set(_ => _.ParentID, folder.Parent.Id)
						.Set(_ => _.Comment, folder.Comment)
					.Update();
		}

		public void Update(FavoritesLink link)
		{
			using (var db = _provider.CreateDBContext())
				db
					.FavoriteItems(fi => fi.ID == link.Id)
						.Set(_ => _.Comment, link.Comment)
						.Set(_ => _.Url, link.Url)
					.Update();
		}

		public void Delete(FavoritesFolder folder)
		{
			var parentFolder = (FavoritesFolder)folder.Parent;

			var ids = new List<int>(GetSubFoldersIds(folder)) {folder.Id};

			using (var db = _provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				var splits = ids.SplitForInClause(_provider);
				foreach (var series in splits)
					// ReSharper disable AccessToModifiedClosure
					db.FavoriteItems(fi => series.Contains(fi.FolderID)).Delete();
				// ReSharper restore AccessToModifiedClosure

				foreach (var series in splits)
					// ReSharper disable AccessToModifiedClosure
					db.FavoriteFolders(ff => series.Contains(ff.ID)).Delete();
				// ReSharper restore AccessToModifiedClosure

				tx.Commit();
			}

			parentFolder.SubFolders.Remove(folder);
			SetFavoriteLinksSetDirty();
		}

		private static IEnumerable<int> GetSubFoldersIds(FavoritesFolder folder)
		{
			if (folder == null)
				throw new ArgumentNullException("folder");

			foreach (var subFolder in folder.SubFolders)
			{
				yield return subFolder.Id;
				foreach (var fid in GetSubFoldersIds(subFolder))
					yield return fid;
			}
		}

		public void Delete(FavoritesLink link)
		{
			var parentFolder = (FavoritesFolder)link.Parent;
			using (var db = _provider.CreateDBContext())
				db.FavoriteItems(fi => fi.ID == link.Id).Delete();
			parentFolder.Links.Remove(link);
			SetFavoriteLinksSetDirty();
		}

		public bool Move(
			FavoritesLink msg,
			FavoritesFolder newParent)
		{
			if (FindByMessageId(newParent.Links, msg.MessageId) != null)
				return false;

			var oldParent = (FavoritesFolder)msg.Parent;
			// здесь, т.к. в Links.Add() Parent будет переустановлен

			newParent.Links.Add(msg);

			using (var db = _provider.CreateDBContext())
				db
					.FavoriteItems(fi => fi.ID == ((IFavoritesEntry)msg).Id)
						.Set(_ => _.FolderID, newParent.Id)
					.Update();
			oldParent.Links.Remove(msg);

			newParent.SortLinks(Config.Instance.FavoritesMessagesSortCriteria);

			return true;
		}

		public bool Move(FavoritesFolder folder, FavoritesFolder newParent)
		{
			var parentFolder = (FavoritesFolder)folder.Parent;
			parentFolder.SubFolders.Remove(folder);
			newParent.SubFolders.Add(folder);
			folder.Update();

			newParent.SortFolders(Config.Instance.FavoritesFoldersSortDirection);

			return true;
		}

		private static FavoritesFolder CreateFavoritesFolders(
			IDictionary<int, FavoritesFolder> favorites)
		{
			var rootFolder = favorites[0];
			// FavoritesFolder.CreateInstance(0, string.Empty, string.Empty, null);

			foreach (var folder in favorites.Values)
			{
				if (folder.Id == 0)
					continue;

				var parent = favorites[folder.ParentId];

				parent.SubFolders.Add(folder);
				folder.Parent = parent;
			}

			return rootFolder;
		}

		private void OnReload()
		{
			if (FavoritesReloaded != null)
				FavoritesReloaded(this, EventArgs.Empty);
		}

		private static FavoritesLink FindByMessageId(
			IEnumerable<FavoritesLink> links, int messageId)
		{
			return links.FirstOrDefault(link => link.MessageId == messageId);
		}

		public bool IsFavorite(int mid)
		{
			return FavoriteLinksSet.ContainsKey(mid);
		}

		#region FavoriteLinksSet
		private Dictionary<int, bool> _favoriteLinksSet;

		private Dictionary<int, bool> FavoriteLinksSet
		{
			get
			{
				if (_favoriteLinksSet == null)
				{
					_favoriteLinksSet = new Dictionary<int, bool>();

					foreach (var mid in GetFavoriteLinks(RootFolder))
						if (!_favoriteLinksSet.ContainsKey(mid))
							_favoriteLinksSet.Add(mid, true);
				}
				return _favoriteLinksSet;
			}
		}

		/// <summary>
		/// Пометить набор избранных сообщений как невалидный.
		/// </summary>
		private void SetFavoriteLinksSetDirty()
		{
			_favoriteLinksSet = null;
		}

		private static IEnumerable<int> GetFavoriteLinks(FavoritesFolder folder)
		{
			if (folder == null)
				throw new ArgumentNullException("folder");

			foreach (var link in folder.Links)
				yield return link.MessageId;

			foreach (var subFolder in folder.SubFolders)
				foreach (var mid in GetFavoriteLinks(subFolder))
					yield return mid;
		}
		#endregion

		public FavoritesManager(IServiceProvider provider)
		{
			_provider = provider;
			Reload();
		}
	}
}