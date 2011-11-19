using System;
using System.Collections.Generic;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	internal class FavoritesDummyFormService : IFavoritesDummyFormService
	{
		private readonly FavoritesDummyForm _form;

		public FavoritesDummyFormService(FavoritesDummyForm form)
		{
			if (form == null)
				throw new ArgumentNullException("form");

			_form = form;
		}

		#region IFavoritesDummyFormService Members

		public IList<IFavoritesEntry> SelectedEntries
		{
			get { return _form.SelectedEntries; }
		}

		public event EventHandler SelectedEntriesChanged
		{
			add { _form.SelectedEntriesChanged += value; }
			remove { _form.SelectedEntriesChanged -= value; }
		}

		public void Refresh()
		{
			_form.Refresh();
		}

		public ITreeNode LastActiveNode
		{
			get { return _form.LastActiveNode; }
			set { _form.LastActiveNode = value; }
		}

		#endregion
	}
}
