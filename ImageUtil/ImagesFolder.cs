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

namespace ImageUtil
{
	public class ImagesFolder : ITreeNode
	{
		private static readonly char _nameSeparator = Path.DirectorySeparatorChar;

		private readonly string _name;
		private readonly ImagesFolder _parentFolder;
		private readonly List<ImagesFolder> _folders = new List<ImagesFolder>();
		private readonly Dictionary<string, ImageInfo> _images = new Dictionary<string, ImageInfo>();

		private ImagesFolder(string name, ImagesFolder parent)
		{
			_name = name;
			_parentFolder = parent;
			Parent = parent;
			Flags = NodeFlags.None;
			IsChanged = false;
		}

		public ImagesFolder()
			: this("<root>", null)
		{ }

		public ImagesFolder(IResourceReader resources)
			: this("<root>", null)
		{
			foreach (DictionaryEntry e in resources)
			{
				if (!(e.Value is Image))
					continue;

				AddImage(e.Key.ToString(), (Image)e.Value);
			}

			IsChanged = false;
		}

		public void Save(IResourceWriter resources)
		{
			foreach (var info in _images.Values)
				resources.AddResource(info.FullName, info.Image);
			foreach (var folder in _folders)
				folder.Save(resources);

			IsChanged = false;
		}

		public static char NameSeparator
		{
			get { return _nameSeparator; }
		}

		public string Name
		{
			get { return _name; }
		}

		public bool IsRoot
		{
			get { return _parentFolder == null; }
		}

		public int ImagesCount
		{
			get { return _images.Count; }
		}

		public IEnumerable<ImageInfo> Images
		{
			get { return _images.Values; }
		}

		public bool IsChanged { get; private set; }

		public event EventHandler<ImageNameArgs> ImageAdded;
		public event EventHandler<ImageNameArgs> ImageRemoved;
		public event EventHandler<ImageRenamedArgs> ImageRenamed;
		public event EventHandler<ImageAddConflictArgs> ImageAddConflict;

		public string GetFullName(string shortName)
		{
			var names = new List<string>();

			var folder = this;
			while (!folder.IsRoot)
			{
				names.Add(folder.Name);
				folder = folder._parentFolder;
			}

			var sb = new StringBuilder();
			for (var i = names.Count - 1; i >= 0; --i)
			{
				sb.Append(names[i]);
				sb.Append(_nameSeparator);
			}
			sb.Append(shortName);

			return sb.ToString();
		}

		private ImagesFolder GetRootFolder()
		{
			var folder = this;
			while (!folder.IsRoot)
				folder = folder._parentFolder;
			return folder;
		}

		public int GetAllImagesCount()
		{
			return ImagesCount + _folders.Sum(folder => folder.GetAllImagesCount());
		}

		public ImageList CreateImageList()
		{
			var il = new ImageList();

			if (_images.Count > 0)
			{
				int maxw = 0, maxh = 0;
				foreach (var info in _images.Values)
				{
					if (maxw < info.Image.Width)
						maxw = info.Image.Width;
					if (maxh < info.Image.Height)
						maxh = info.Image.Height;
				}
				if (maxw > 64)
					maxw = 64;
				if (maxh > 64)
					maxh = 64;

				il.ImageSize = new Size(maxw, maxh);

				foreach (var info in _images.Values)
				{
					var scale = Math.Min((float)maxw / info.Image.Width, (float)maxh / info.Image.Height);
					var w = scale < 1.0f ? (int)(scale * info.Image.Width) : info.Image.Width;
					var h = scale < 1.0f ? (int)(scale * info.Image.Height) : info.Image.Height;
					var x = (maxw - w) / 2;
					var y = (maxh - h) / 2;

					Image img = new Bitmap(maxw, maxh);
					using (var g = Graphics.FromImage(img))
						g.DrawImage(info.Image, x, y, w, h);
					il.Images.Add(info.ShortName, img);
				}
			}

			return il;
		}

		public ImageInfo GetImageInfo(string shortName)
		{
			return _images[shortName];
		}

		public bool HasImage(string relativeName)
		{
			return HasImage(relativeName.Split(_nameSeparator));
		}

		public void AddImage(string fullName, Image image)
		{
			var names = fullName.Split(_nameSeparator);
			if (names.Length != 0)
			{
				var root = GetRootFolder();
				root.AddImage(fullName, names, image);
				root.SetChanged();
			}
		}

		public void RemoveImage(string shortName)
		{
			var info = _images[shortName];
			_images.Remove(shortName);
			OnImageRemoved(info.FullName, info.ShortName);
			SetChanged();
		}

		private void SetChanged()
		{
			IsChanged = true;
			if (_parentFolder != null)
				_parentFolder.SetChanged();
		}

		public void RenameImage(string shortName, string fullName)
		{
			var info = _images[shortName];
			if (info.FullName == fullName)
				return;
			RemoveImage(shortName);
			GetRootFolder().AddImage(fullName, info.Image);
			OnImageRenamed(info.FullName, fullName);
			SetChanged();
		}

		private bool HasImage(string[] names)
		{
			var shortName = names[0];

			if (names.Length == 1)
				return _images.ContainsKey(shortName);

			var folder = _folders.FirstOrDefault(f => f.Name == shortName);
			if (folder == null)
				return false;

			var subNames = new string[names.Length - 1];
			Array.Copy(names, 1, subNames, 0, names.Length - 1);
			return folder.HasImage(subNames);
		}

		private void AddImage(string fullName, string[] names, Image image)
		{
			var shortName = names[0];

			if (names.Length == 1)
			{
				if (_images.ContainsKey(shortName))
				{
					if (GetRootFolder().OnAddConflict(shortName, fullName))
					{
						_images[shortName] = new ImageInfo(fullName, shortName, image);
						// TODO: Сделать более красиво.
						OnImageRemoved(fullName, shortName);
						OnImageAdded(fullName, shortName);
					}
				}
				else
				{
					_images.Add(shortName, new ImageInfo(fullName, shortName, image));
					OnImageAdded(fullName, shortName);
				}
				return;
			}

			var folder = _folders.FirstOrDefault(f => f.Name == shortName);
			if (folder == null)
			{
				folder = new ImagesFolder(shortName, this);
				_folders.Add(folder);
			}

			var subNames = new string[names.Length - 1];
			Array.Copy(names, 1, subNames, 0, names.Length - 1);
			folder.AddImage(fullName, subNames, image);
		}

		private void OnImageAdded(string fullName, string shortName)
		{
			if (ImageAdded != null)
				ImageAdded(this, new ImageNameArgs(shortName, fullName));
		}

		private void OnImageRemoved(string fullName, string shortName)
		{
			if (ImageRemoved != null)
				ImageRemoved(this, new ImageNameArgs(shortName, fullName));
		}

		private void OnImageRenamed(string prevFullName, string newFullName)
		{
			if (ImageRenamed != null)
				ImageRenamed(this, new ImageRenamedArgs(prevFullName, newFullName));
		}

		private bool OnAddConflict(string shortName, string fullName)
		{
			var ea = new ImageAddConflictArgs(shortName, fullName);
			if (ImageAddConflict != null)
				ImageAddConflict(this, ea);
			return !ea.Cancel;
		}

		#region ITreeNode Members

		public ITreeNode Parent { get; set; }

		public NodeFlags Flags { get; set; }

		public bool HasChildren
		{
			get { return _folders.Count > 0; }
		}

		public ITreeNode this[int iIndex]
		{
			get { return _folders[iIndex]; }
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			if (!(array is ImagesFolder[]))
				throw new ArgumentException();
			_folders.CopyTo((ImagesFolder[])array, index);
		}

		public int Count
		{
			get { return _folders.Count; }
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
			return _folders.GetEnumerator();
		}

		#endregion
	}
}
