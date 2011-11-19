using System.Collections.Generic;
using Rsdn.TreeGrid;
using System;

namespace Rsdn.Janus
{
	public interface IFavoritesDummyFormService
	{
		IList<IFavoritesEntry> SelectedEntries { get; }
		event EventHandler SelectedEntriesChanged;

		void Refresh();
		ITreeNode LastActiveNode { get; set; }
	}
}