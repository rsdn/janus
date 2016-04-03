using System;
using System.Windows.Forms;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for SelectFavoritesFolderForm.
	/// </summary>
	public partial class FavoritesSelectFolderForm : Form
	{
		private readonly IFavoritesManager _favManager;

		#region Constructor(s) & Dispose
		public FavoritesSelectFolderForm(
			IServiceProvider provider,
			FavoritesFolder folders, bool newItem)
		{
			_favManager = provider.GetRequiredService<IFavoritesManager>();
			InitializeComponent();

			if (!newItem)
			{
				_comment.Visible = false;
				_commentLabel.Visible = false;
			}

			_folderView.SmallImageList = FavoritesDummyForm.Instance.TreeImages;

			if (folders.ShowLinks)
				folders.ShowLinks = false;
			_folderView.Nodes = folders;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			var rootFolder = (FavoritesFolder)_folderView.Nodes;

			if (!rootFolder.ShowLinks)
				rootFolder.ShowLinks = true;

			base.Dispose(disposing);
		}

		#endregion

		#region Properties
		public FavoritesFolder SelectedFolder => (FavoritesFolder) _folderView.ActiveNode;

		public string Comment
		{
			get { return _comment.Text; }
			set { _comment.Text = value; }
		}
		#endregion

		#region Control events
		private void CreateFolderButtonClick(object sender, EventArgs e)
		{
			using (var pb = new FavoritesFolderForm(string.Empty, string.Empty, true))
				if (pb.ShowDialog(this) == DialogResult.OK)
				{
					var activeFolder = _folderView.ActiveNode as FavoritesFolder;
					if (activeFolder == null || pb.CreateAsRoot)
						activeFolder = _favManager.RootFolder;

					// TODO: Действия в AddFolder и Reload, приводят к тому,
					// что добавление первой папки вызывает искючение при установке свойства Nodes дерева.
					_favManager.AddFolder(pb.FolderName, pb.FolderComment, activeFolder);
					_favManager.Reload();

					var folders = _favManager.RootFolder;
					folders.ShowLinks = false;

					_folderView.VirtualListSize = folders.Count;
					_folderView.Nodes = folders;
				}
		}
		#endregion
	}
}