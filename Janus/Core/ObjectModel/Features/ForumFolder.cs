using System;

using Rsdn.TreeGrid;

namespace Rsdn.Janus.ObjectModel
{
	/// <summary>
	/// Папка форумов.
	/// </summary>
	internal class ForumFolder : FolderFeature
	{
		public ForumFolder(IServiceProvider provider, ITreeNode parent, string name)
			: base(provider)
		{
			_parent = parent;
			_description = name;
		}

		public override int ImageIndex
		{
			get
			{
				return ObjectModel.Features.Instance.UnsubscribedFolderImageIndex;
			}
		}

		public void Add(Forum forum)
		{
			forum._parent = this;
			Features.Add(forum);
		}

		public void Clear()
		{
			Features.Clear();
		}
	}
}
