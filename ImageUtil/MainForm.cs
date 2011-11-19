using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;

using Rsdn.TreeGrid;
using Rsdn.Janus.Framework.Imaging;

namespace ImageUtil
{
	public partial class MainForm : Form
	{
		private ImagesFolder _rootFolder;
		private string _fileName;
		private readonly Dictionary<string, Image> _loadedImages = new Dictionary<string, Image>();
		private ImagesFolder _activeFolder;
		private bool _suppressImagesListUpdate;
		private string _draggingImage;

		public MainForm()
		{
			InitializeComponent();

			var sb = new StringBuilder();
			sb.Append("All image formats|");
			var first = true;
			foreach (var format in ImageFormatInfo.Formats)
			{
				if (!first)
					sb.Append(";");
				sb.AppendFormat("*.{0}", format.Extension);
				first = false;
			}
			sb.Append("|All files|*.*");

			_openImageDialog.Filter = sb.ToString();

			UpdateButtonsStates();
		}

		#region Handlers

		private void ExitMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		private void OpenMenuItemClick(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void OpenButtonClick(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void SaveAsMenuItemClick(object sender, EventArgs e)
		{
			SaveFileAs();
		}

		private void SaveMenuItemClick(object sender, EventArgs e)
		{
			SaveFile();
		}

		private void SaveButtonClick(object sender, EventArgs e)
		{
			SaveFile();
		}

		private void AddContextMenuItemClick(object sender, EventArgs e)
		{
			AddImage();
		}

		private void RemoveContextMenuItemClick(object sender, EventArgs e)
		{
			RemoveImages();
		}

		private void RenameContextMenuItemClick(object sender, EventArgs e)
		{
			RenameImage();
		}

		private void AddMenuItemClick(object sender, EventArgs e)
		{
			AddImage();
		}

		private void RemoveMenuItemClick(object sender, EventArgs e)
		{
			RemoveImages();
		}

		private void RenameMenuItemClick(object sender, EventArgs e)
		{
			RenameImage();
		}

		private void AddButtonClick(object sender, EventArgs e)
		{
			AddImage();
		}

		private void RemoveButtonClick(object sender, EventArgs e)
		{
			RemoveImages();
		}

		private void RenameButtonClick(object sender, EventArgs e)
		{
			RenameImage();
		}

		private void SaveToFileMenuItemClick(object sender, EventArgs e)
		{
			SaveImageToFile();
		}

		private void SaveToFileContextMenuItemClick(object sender, EventArgs e)
		{
			SaveImageToFile();
		}

		private void ImportFolderContextMenuItemClick(object sender, EventArgs e)
		{
			ImportFolder();
		}

		private void ImportFolderMenuItemClick(object sender, EventArgs e)
		{
			ImportFolder();
		}

		private void FoldersTreeGetData(object sender, GetDataEventArgs e)
		{
			var folder = (ImagesFolder)e.Node;
			e.CellInfos[0].Text = folder.Name;
			e.CellInfos[1].Text = string.Format("{0}/{1}", folder.ImagesCount, folder.GetAllImagesCount());
		}

		private void FoldersTreeAfterActivateNode(ITreeNode activatedNode)
		{
			var node = _foldersTree.ActiveNode;
			if (_activeFolder != null)
			{
				_activeFolder.ImageAdded -= ImagesFolder_ImageAdded;
				_activeFolder.ImageRemoved -= ImagesFolder_ImageRemoved;
			}
			if (node != null)
			{
				_activeFolder = (ImagesFolder)node;
				_activeFolder.ImageAdded += ImagesFolder_ImageAdded;
				_activeFolder.ImageRemoved += ImagesFolder_ImageRemoved;

				FillList(_activeFolder);
			}
			else
			{
				_activeFolder = null;
			}

			UpdateButtonsStates();
		}

		private void FoldersTreeMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				_removeContextMenuItem.Visible = false;
				_renameContextMenuItem.Visible = false;
				_renameFolderContextMenuItem.Visible = _activeFolder != null && !_activeFolder.IsRoot;
				_saveToFileContextMenuItem.Visible = false;
				_contextMenu.Show(_foldersTree, e.Location);
			}
		}

		private void ImagesListMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				_removeContextMenuItem.Visible = _imagesList.SelectedItems.Count != 0;
				_renameContextMenuItem.Visible = _imagesList.SelectedItems.Count == 1;
				_renameFolderContextMenuItem.Visible = false;
				_saveToFileContextMenuItem.Visible = _imagesList.SelectedItems.Count == 1;
				_contextMenu.Show(_imagesList, e.Location);
			}
		}

		private void ImagesListAfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (!IsNameValid(e.Label) || e.Label.IndexOf(ImagesFolder.NameSeparator) != -1)
			{
				e.CancelEdit = true;
				return;
			}

			using (new TreeGridUpdater(_foldersTree))
				_activeFolder.RenameImage(_imagesList.Items[e.Item].Name, _activeFolder.GetFullName(e.Label));
		}

		private void ImagesListSelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateButtonsStates();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!TrySaveChanges())
				e.Cancel = true;
		}

		private void ImagesFolder_ImageAdded(object sender, ImageNameArgs e)
		{
			if (_suppressImagesListUpdate)
				return;

			var folder = (ImagesFolder)sender;
			UpdateImagesListImageList(folder);
			_imagesList.Items.Add(CreateImageItem(folder.GetImageInfo(e.ShortName)));
		}

		private void ImagesFolder_ImageRemoved(object sender, ImageNameArgs e)
		{
			if (_suppressImagesListUpdate)
				return;

			var folder = (ImagesFolder)sender;
			UpdateImagesListImageList(folder);
			_imagesList.Items.RemoveByKey(e.ShortName);
		}

		void RootFolderImageAddConflict(object sender, ImageAddConflictArgs e)
		{
			if(MessageBox.Show(string.Format("{0} is already exists.\nOverwrite it?", e.FullName),
				Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
				e.Cancel = true;
		}

		#endregion

		#region Drag & Drop

		private void ImagesListDragEnter(object sender, DragEventArgs e)
		{
			try
			{
				_draggingImage = null;

				if (e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					var files = (string[])e.Data.GetData(DataFormats.FileDrop);
					if (files.Length == 1)
					{
						var fileName = files[0];
						if (!_loadedImages.ContainsKey(fileName))
							_loadedImages.Add(fileName, Image.FromFile(fileName));
						_draggingImage = fileName;
					}
				}
			}
			catch
			{
				_draggingImage = null;
			}
			e.Effect = _draggingImage != null ? e.AllowedEffect & DragDropEffects.Copy : DragDropEffects.None;
		}

		private void ImagesListDragLeave(object sender, EventArgs e)
		{
			_draggingImage = null;
		}

		private void ImagesListDragDrop(object sender, DragEventArgs e)
		{
			if (_draggingImage != null)
			{
				AddImage(_draggingImage);
				_draggingImage = null;
			}
		}

		#endregion

		#region Commands

		private void ImportFolder()
		{
			using (var fbd = new FolderBrowserDialog())
				if (fbd.ShowDialog() == DialogResult.OK)
				{
					if (_rootFolder == null)
					{
						_rootFolder = new ImagesFolder();
						_rootFolder.ImageAddConflict += RootFolderImageAddConflict;
						ImportFolder(_rootFolder, fbd.SelectedPath);

						AssignRootFolder();
					}
					else
					{
						using (new TreeGridUpdater(_foldersTree))
						{
							_suppressImagesListUpdate = true;
							try
							{
								ImportFolder(_activeFolder ?? _rootFolder, fbd.SelectedPath);
							}
							finally
							{
								_suppressImagesListUpdate = false;
							}
						}
					}
				}
		}

		private void ImportFolder(ImagesFolder folder, string folderPath)
		{
			var files = new Dictionary<string, List<string>>();

			LoadImages(folderPath, false, string.Empty, files);

			foreach (var kvp in files)
			{
				var prefix = string.IsNullOrEmpty(kvp.Key) ? string.Empty : kvp.Key + ImagesFolder.NameSeparator;
				foreach (var file in kvp.Value)
				{
					var shortName = Path.GetFileNameWithoutExtension(file);
					folder.AddImage(folder.GetFullName(prefix + shortName), _loadedImages[file]);
				}
			}
		}

		private void LoadImages(string folderPath, bool importHidden, string prefix, Dictionary<string, List<string>> files)
		{
			var imageFiles = new List<string>();

			foreach (var file in Directory.GetFiles(folderPath))
				try
				{
					Image img;
					if (!_loadedImages.TryGetValue(file, out img))
					{
						img = Image.FromFile(file);
						_loadedImages.Add(file, img);
					}
					imageFiles.Add(file);
				}
				catch
				{}

			if (imageFiles.Count > 0)
				files.Add(prefix, imageFiles);

			var namePrefix = string.IsNullOrEmpty(prefix) ? string.Empty : prefix + ImagesFolder.NameSeparator;
			foreach (var directory in Directory.GetDirectories(folderPath))
			{
				if(!importHidden)
				{
					var info = new DirectoryInfo(directory);
					if((info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
						continue;
				}
				var shortName = Path.GetFileNameWithoutExtension(directory);
				LoadImages(directory, importHidden, namePrefix + shortName, files);
			}
		}

		private void OpenFile()
		{
			if (!TrySaveChanges())
				return;

			if (_openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (_rootFolder != null)
				{
					_rootFolder.ImageAddConflict -= RootFolderImageAddConflict;
					_rootFolder = null;

					DisposeLoadedImages();
				}

				var fileName = _openFileDialog.FileName;
				if (fileName.ToLower().Contains(".resx"))
					using (IResourceReader rd = new ResXResourceReader(fileName))
						_rootFolder = new ImagesFolder(rd);
				else
					using (IResourceReader rd = new ResourceReader(fileName))
						_rootFolder = new ImagesFolder(rd);

				_rootFolder.ImageAddConflict += RootFolderImageAddConflict;
				_fileName = fileName;

				AssignRootFolder();
			}
		}

		private bool TrySaveChanges()
		{
			if (_rootFolder != null && _rootFolder.IsChanged)
			{
				switch (MessageBox.Show("Images was changed\nSave them before close?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
				{
					case DialogResult.Cancel:
						return false;
					case DialogResult.Yes:
						SaveFile();
						break;
				}
			}
			return true;
		}

		private void SaveFile()
		{
			if (string.IsNullOrEmpty(_fileName))
			{
				SaveFileAs();
			}
			else if (_fileName.ToLower().Contains(".resx"))
			{
				using (IResourceWriter rw = new ResXResourceWriter(_fileName))
					_rootFolder.Save(rw);
			}
			else
			{
				using (IResourceWriter rw = new ResourceWriter(_fileName))
					_rootFolder.Save(rw);
			}
		}

		private void SaveFileAs()
		{
			if (_saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				_fileName = _saveFileDialog.FileName;
				SaveFile();
			}
		}

		private void AddImage()
		{
			if (_openImageDialog.ShowDialog() == DialogResult.OK)
				AddImage(_openImageDialog.FileName);
		}

		void ImageNameForm_ImageNameChanged(object sender, ImageNameChangedEventArgs e)
		{
			e.Cancel = !IsNameValid(e.ImageName) || _rootFolder.HasImage(e.ImageName);
		}

		private static bool IsNameValid(string name)
		{
			if (string.IsNullOrEmpty(name))
				return false;
			if (name[name.Length - 1] == ImagesFolder.NameSeparator)
				return false;
			var parts = name.Split(ImagesFolder.NameSeparator);
			if (parts.Length == 0)
				return false;
			return parts.All(part => part.IndexOfAny(Path.GetInvalidPathChars()) == -1);
		}

		private void RemoveImages()
		{
			using (new TreeGridUpdater(_foldersTree))
			{
				_suppressImagesListUpdate = true;
				try
				{
					var shortNames = new List<string>(_imagesList.SelectedItems.Count);
					shortNames
						.AddRange(
							_imagesList
								.SelectedItems
								.Cast<ListViewItem>()
								.Select(lvi => lvi.ImageKey));
					foreach (var shortName in shortNames)
						_activeFolder.RemoveImage(shortName);
				}
				finally
				{
					_suppressImagesListUpdate = false;
					FillList(_activeFolder);
				}
			}
		}

		private void RenameImage()
		{
			if (_imagesList.SelectedItems.Count != 1)
				return;

			var info = _activeFolder.GetImageInfo(_imagesList.SelectedItems[0].Name);

			using (var inf = new ImageNameForm())
			{
				inf.ImageNameChanged += ImageNameForm_ImageNameChanged;
				inf.ImageName = info.FullName;

				if (inf.ShowDialog() == DialogResult.OK && info.FullName != inf.ImageName)
					using (new TreeGridUpdater(_foldersTree))
						_activeFolder.RenameImage(info.ShortName, inf.ImageName);
			}
		}

		private void SaveImageToFile()
		{
			if (_imagesList.SelectedItems.Count != 1)
				return;

			var info = _activeFolder.GetImageInfo(_imagesList.SelectedItems[0].Name);

			using (var sfd = new SaveFileDialog())
			{
				var fi = ImageFormatInfo.FromImageFormat(info.Image.RawFormat);
				sfd.FileName = string.Format("{0}.{1}", info.ShortName, fi.Extension);
				sfd.Filter = string.Format("{0} images (*.{0})|*.{0}|All files|*.*", fi.Extension);
				sfd.FilterIndex = 1;
				if (sfd.ShowDialog() == DialogResult.OK)
					try
					{
						info.Image.Save(sfd.FileName);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
			}
		}

		#endregion

		#region Helpers

		private void UpdateButtonsStates()
		{
			var canSaveFile = _rootFolder != null;

			_saveButton.Enabled = canSaveFile;
			_saveMenuItem.Enabled = canSaveFile;
			_saveAsMenuItem.Enabled = canSaveFile;

			var hasActiveImages = _imagesList.SelectedItems.Count > 0;

			_removeButton.Enabled = hasActiveImages;
			_removeMenuItem.Enabled = hasActiveImages;

			var hasActiveImage = _imagesList.SelectedItems.Count == 1;

			_renameButton.Enabled = hasActiveImage;
			_renameMenuItem.Enabled = hasActiveImage;

			_saveToFileMenuItem.Enabled = hasActiveImage;
		}

		private void UpdateImagesListImageList(ImagesFolder folder)
		{
			var il = _imagesList.LargeImageList;
			_imagesList.LargeImageList = folder.CreateImageList();
			if (il != null)
				il.Dispose();
		}

		private static ListViewItem CreateImageItem(ImageInfo info)
		{
			var lvi = new ListViewItem(info.ShortName, info.ShortName) {Name = info.ShortName};
			var fi = ImageFormatInfo.FromImageFormat(info.Image.RawFormat);
			lvi.ToolTipText = string.Format("{0} {1}x{2}", fi.Extension, info.Image.Width, info.Image.Height);
			return lvi;
		}

		private void FillList(ImagesFolder folder)
		{
			_imagesList.BeginUpdate();
			try
			{
				_imagesList.Items.Clear();
				if (folder == null)
					return;

				UpdateImagesListImageList(folder);

				foreach (var info in folder.Images)
					_imagesList.Items.Add(CreateImageItem(info));
			}
			finally
			{
				_imagesList.EndUpdate();
			}
		}

		private void AssignRootFolder()
		{
			var root = new RootNode(_rootFolder);
			_rootFolder.Parent = root;
			_foldersTree.Nodes = root;
			_foldersTree.ActiveNode = _rootFolder;
		}

		private void AddImage(string imageFileName)
		{
			try
			{
				Image img;
				if (!_loadedImages.TryGetValue(imageFileName, out img))
				{
					img = Image.FromFile(imageFileName);
					_loadedImages.Add(imageFileName, img);
				}

				using (var inf = new ImageNameForm())
				{
					if (_activeFolder != null)
					{
						inf.ImageNameChanged += ImageNameForm_ImageNameChanged;
						inf.ImageName = _activeFolder.GetFullName(
							Path.GetFileNameWithoutExtension(imageFileName));
					}

					if (inf.ShowDialog() == DialogResult.OK)
					{
						if (_rootFolder == null)
						{
							_rootFolder = new ImagesFolder();
							_rootFolder.ImageAddConflict += RootFolderImageAddConflict;
							_rootFolder.AddImage(inf.ImageName, img);

							AssignRootFolder();
						}
						else
						{
							using (new TreeGridUpdater(_foldersTree))
								_rootFolder.AddImage(inf.ImageName, img);
						}
					}
				}
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void DisposeLoadedImages()
		{
			foreach (var image in _loadedImages.Values)
				image.Dispose();
			_loadedImages.Clear();
		}

		#endregion

		private class TreeGridUpdater : IDisposable
		{
			private readonly ITreeNode _root;
			private readonly TreeGrid _tree;
			private readonly ITreeNode _activeNode;

			public TreeGridUpdater(TreeGrid tree)
			{
				_tree = tree;
				_root = tree.Nodes;
				_activeNode = tree.ActiveNode;
				tree.Nodes = null;
			}

			#region IDisposable Members

			public void Dispose()
			{
				_tree.Nodes = _root;
				_tree.ActiveNode = _activeNode;
			}

			#endregion
		}

		private class RootNode : ITreeNode
		{
			private readonly ImagesFolder _folder;

			public RootNode(ImagesFolder folder)
			{
				if (folder == null)
					throw new ArgumentNullException("folder");
				_folder = folder;
			}

			#region ITreeNode Members

			public ITreeNode Parent
			{
				get { return null; }
			}

			public NodeFlags Flags { get; set; }

			public bool HasChildren
			{
				get { return true; }
			}

			public ITreeNode this[int iIndex]
			{
				get
				{
					if (iIndex != 0)
						throw new IndexOutOfRangeException("iIndex");
					return _folder;
				}
			}

			#endregion

			#region ICollection Members

			public void CopyTo(Array array, int index)
			{
				Array.Copy(new ITreeNode[] { _folder }, array, index);
			}

			public int Count
			{
				get { return 1; }
			}

			public object SyncRoot
			{
				get { return this; }
			}

			public bool IsSynchronized
			{
				get { return false; }
			}

			#endregion

			#region IEnumerable Members

			public IEnumerator GetEnumerator()
			{
				yield return _folder;
			}

			#endregion
		}
	}
}