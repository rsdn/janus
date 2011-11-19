using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Peak.TreeGrid
{
	public class TreeGrid1<T>: TreeGrid1, ITreeGrid<T>
	{
		ITreeModel<T> realModel;

		#region ITreeGrid<T> Members
		[DefaultValue(null)]
		public ITreeModel<T> TreeModel
		{
			get
			{
				return realModel;
			}
			set
			{
				realModel = value;
				if (value != null)
				{
					TreeStructure newS = new TreeStructure();
					newS.Owner = this;
					newS.data = realModel.Root;
					newS.VisibleCount = 0;
					newS.Expanded = true;
					realGrid.RowCount = newS.VisibleCount;
					tree = newS;
				}
				else
				{
					realGrid.RowCount = 0;
				}
			}
		}

		ICollection<Column> ITreeGrid<T>.Columns
		{
			get { return Columns; }
		}
		#endregion

		protected override CellInfo DoGetCellInfo(int row, int column)
		{
			try
			{
				T obj = GetRowObject(row);
				if (obj != null)
				{
					string dataProp = Columns[column].DataPropertyName;
					IObjectInfo<T> objInfo = TreeModel.ObjectInfos[obj];
					if (objInfo != null)
					{
						CellInfo ret = objInfo.Cells[dataProp, DoGetVisualInfo(row, column)];
						if (ret != null)
							return ret;
					}
				}

			}
			catch 
			{
			} 
			return base.DoGetCellInfo(row, column);
		}

		int levelIdent = 16;

		[DefaultValue(16)]
		[DisplayName("Level Ident")]
		public int LevelIdent
		{
			get { return levelIdent; }
			set
			{
				if (value < 1 || value > 32)
					return;
				levelIdent = value;
			}
		}

		protected override int DoGetIdent(int row, int column)
		{
			if (column != 0)
				return 0;
			TreeStructure node = tree.GetVisibleChildData(row);
			if (node != null)
				return node.Level * LevelIdent;
			return base.DoGetIdent(row, column);
		}

		protected override StyleInfo DoGetStyleInfo(int row, int column)
		{
			try
			{
				T obj = GetRowObject(row);
				if (obj != null)
				{
					string dataProp = Columns[column].DataPropertyName;
					IObjectInfo<T> objInfo = TreeModel.ObjectInfos[obj];
					if (objInfo != null)
					{
						StyleInfo ret = objInfo.Styles[dataProp, DoGetVisualInfo(row, column)];
						if (ret != null)
							return ret;
					}
				}
			}
			catch
			{
			}
			return base.DoGetStyleInfo(row, column);
		}

		protected override bool? DoGetExpanded(int row, int column)
		{
			TreeStructure ts = tree.GetVisibleChildData(row);
			if (ts != null && ts.Count != 0)
			{
				return ts.Expanded;
			}
			return base.DoGetExpanded(row, column);
		}

		protected override bool DoDrawExpansion(int column)
		{
			return column == 0;
		}

		protected override void DoExpandCollapse(int row)
		{
			TreeStructure ts = tree.GetVisibleChildData(row);
			if (ts == null)
				return;
			if (ts.Count != 0)
				ts.Expanded = !ts.Expanded;
			realGrid.RowCount = tree.VisibleCount;
			realGrid.Invalidate();
			base.DoExpandCollapse(row);
		}

		TreeStructure tree = null;

		private class TreeStructure
		{
			TreeGrid1<T> owner;

			public TreeGrid1<T> Owner
			{
				get { return owner; }
				set { owner = value; }
			}

			int? count = null;

			public int Count
			{
				get
				{
					if (count == null)
					{
						count = owner.TreeModel.ChildCount[data];
					}
					return (int)count;
				}
			}

			internal T data;

			public T Data
			{
				get { return data; }
			}

			TreeStructure parent = null;

			public TreeStructure Parent
			{
				get { return parent; }
				set { parent = value; }
			}

			int visibleCount = 1;

			public int VisibleCount
			{
				get { return visibleCount; }
				set { visibleCount = value; }
			}

			List<TreeStructure> children = new List<TreeStructure>();

			public TreeStructure this[int index]
			{
				get
				{
					if (children.Count == 0 && Count != 0)
					{
						for (int i = 0; i < Count; i++)
						{
							children.Add(null);
						}
					}
					if (children.Count > index)
					{
						if (children[index] == null)
						{
							children[index] = new TreeGrid1<T>.TreeStructure();
							children[index].data = owner.TreeModel.Children[data, index];
							children[index].parent = this;
							children[index].level = level + 1;
							children[index].owner = owner;
						}
						return children[index];
					}
					return null;
				}
			}

			int level = -1;

			public int Level
			{
				get { return level; }
				set { level = value; }
			}

			bool expanded = false;

			public bool Expanded
			{
				get { return expanded; }
				set 
				{ 
					if (expanded != value)
					expanded = value;
					int oldVCount = VisibleCount;
					if (value)
					{
						for (int i = 0; i < Count; i++)
						{
							visibleCount += this[i].VisibleCount;
						}
					}
					else
					{
						for (int i = 0; i < Count; i++)
						{
							visibleCount -= this[i].VisibleCount;
						}
					}
					TreeStructure par = parent;
					while (par != null)
					{
						par.visibleCount += (visibleCount - oldVCount);
						par = par.parent;
					}
				}
			}

			internal T GetVisibleChild(int row)
			{
				TreeStructure ch = GetVisibleChildData(row);
				if (ch == null)
					return default(T);
				else
					return ch.data;
			}

			internal TreeStructure GetVisibleChildData(int row)
			{
				int i = 0;
				int idx = row;
				if (row == 0 && level >= 0)
					return this;
				else if (Level >= 0)
					idx--;
				while (idx > 0 && idx >= this[i].VisibleCount && i < Count)
				{
					idx -= this[i].VisibleCount;
					i++;
				}
				if (i < Count)
					return this[i].GetVisibleChildData(idx);
				else
					return null;
			}
		}

		private T GetRowObject(int row)
		{
			return tree.GetVisibleChild(row);
		}
	}
}
