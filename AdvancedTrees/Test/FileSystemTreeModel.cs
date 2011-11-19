using System;
using System.IO;

using AdvancedTrees;

using Rsdn.SmartApp;

namespace Test
{
	public class FileSystemTreeModel : ITreeModel<object>
	{
		private static readonly Observable<TreeModelChangedEventArgs<object>> _changed =
			new Observable<TreeModelChangedEventArgs<object>>();

		private readonly DirectoryInfo _root = new DirectoryInfo(@"c:\users\евгений\music");

		#region Implementation of ITreeModel<object>

		public object RootParent
		{
			get { return _root; }
		}

		public IObservable<TreeModelChangedEventArgs<object>> Changed
		{
			get { return _changed; }
		}

		public object GetChild(object parent, int index)
		{
			return ((DirectoryInfo) parent).GetDirectories()[index];
		}

		public int GetChildCount(object parent)
		{
			return ((DirectoryInfo) parent).GetDirectories().Length;
		}

		public bool IsLeaf(object node)
		{
			return ((DirectoryInfo) node).GetDirectories().Length == 0;
		}

		#endregion
	}
}