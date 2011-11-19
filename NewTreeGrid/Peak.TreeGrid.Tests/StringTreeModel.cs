using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Peak.TreeGrid.Tests
{
	public class StringTreeModel: ITreeModel<string>
	{
		#region ITreeModel<string> Members

		public string Root
		{
			get { return @"C:\"; }
		}

		public IDualIndexable<string, int, string> Children
		{
			get
			{
				return new DelegateDualIndexable<string, int, string>(
					delegate(string path, int idx)
					{
						List<string>tmp = new List<string>(Directory.GetDirectories(path));
						tmp.AddRange(Directory.GetFiles(path));
						return tmp[idx];
					},
					delegate(string path, int idx, string path2) { });
			}
		}

		public IIndexable<string, int> ChildCount
		{
			get
			{
				return new DelegateIndexable<string,int>(
				  delegate(string path)
				  {
					  if (Directory.Exists(path))
					  {
						  return Directory.GetDirectories(path).Length + Directory.GetFiles(path).Length;
					  }
					  else
					  {
						  return 0;
					  }
				  },
				  delegate(string path, int idx) { });
			}
		}

		public IIndexable<string, bool> Leaf
		{
			get
			{
				return new DelegateIndexable<string, bool>(
					delegate(string path)
					{
						return ChildCount[path] == 0;
					},
				delegate(string path, bool isLeaf) { });
			}
		}

		class StringObjectInfo: IObjectInfo<string>
		{
			private string path;
			public StringObjectInfo(string path)
			{
				this.path = path;
			}

			#region IObjectInfo<string> Members

			public string Object
			{
				get { return path; }
			}

			public CellInfo GetCellInfo(string DataPropertyName, VisualInfo VisualInfo)
			{
				switch (DataPropertyName)
				{
					case "Name":
						return new CellInfo(Path.GetFileName(path), null);
					case "Path":
						return new CellInfo(path, null);
					case "Attributes":
						return new CellInfo(new FileInfo(path).Attributes.ToString(), null);
					default:
						return new CellInfo(string.Empty, null);
				}
			}

			public StyleInfo GetStyleInfo(string DataPropertyName, VisualInfo VisualInfo)
			{
				return null;
			}

			public IDualIndexable<string, VisualInfo, CellInfo> Cells
			{
				get
				{
					return new DelegateDualIndexable<string, VisualInfo, CellInfo>(
						GetCellInfo,
						delegate(string path, VisualInfo visual, CellInfo cell) { });
				}
			}

			public IDualIndexable<string, VisualInfo, StyleInfo> Styles
			{
				get
				{
					return new DelegateDualIndexable<string, VisualInfo, StyleInfo>(
						GetStyleInfo,
						delegate(string path, VisualInfo visual, StyleInfo style) { });
				}
			}

			#endregion
		}

		public IObjectInfo<string> GetObjectInfo(string Object)
		{
			return new StringObjectInfo(Object);
		}

		public IIndexable<string, IObjectInfo<string>> ObjectInfos
		{
			get { return new DelegateIndexable<string, IObjectInfo<string>>(
				GetObjectInfo, 
				delegate(string path, IObjectInfo<string> val) {}); }
		}

		#endregion
	}
}
