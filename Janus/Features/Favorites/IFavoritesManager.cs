using System;

namespace Rsdn.Janus
{
	public interface IFavoritesManager
	{
		FavoritesFolder RootFolder { get; }
		event EventHandler FavoritesReloaded;
		void Reload();
		void AddFolder(string name, string comment, FavoritesFolder parentFolder);
		bool AddMessageLink(int messageId, string comment, FavoritesFolder folder);
		void AddUrlLink(string url, string comment, FavoritesFolder folder);
		void Update(FavoritesFolder folder);
		void Update(FavoritesLink link);
		void Delete(FavoritesFolder folder);
		void Delete(FavoritesLink link);

		bool Move(
			FavoritesLink msg,
			FavoritesFolder newParent);

		bool Move(FavoritesFolder folder, FavoritesFolder newParent);
		bool IsFavorite(int mid);
	}
}