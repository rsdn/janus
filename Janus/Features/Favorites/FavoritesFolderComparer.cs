using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Comparer для разделов Избранного
	/// </summary>
	internal class FavoritesFolderComparer : IComparer<FavoritesFolder>
	{
		private readonly SortDirection _sortDir;

		public FavoritesFolderComparer(SortDirection sortDir)
		{
			_sortDir = sortDir;
		}

		public int Compare(FavoritesFolder folder1, FavoritesFolder folder2)
		{
			var result = folder1.Name.CompareTo(folder2.Name);
			return (_sortDir == SortDirection.Asc) ? result : -result;
		}
	}
}