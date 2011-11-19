using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Microsoft.CSharp;

using Rsdn.LocUtil.Helper;
using Rsdn.LocUtil.Model;
using Rsdn.LocUtil.Model.Design;

using Rsdn.TreeGrid;
using System.Collections.Generic;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	internal partial class MainForm : Form
	{
		private RootCategory _rootCategory;
		private string _directory;
		private bool _contextMenuEnabled;

		internal MainForm()
		{
			InitializeComponent();
		}

		public override ISite Site
		{
			get { return base.Site; }
			set 
			{
				base.Site = value;
				if (Site != null)
					Site.Container.Add(_propertyGrid);
			}
		}

		private void OpenFile()
		{
			if (_openFileDialog.ShowDialog() == DialogResult.OK)
			{
				CultureInfo[] loadedCultures;
				DateTime lwt;
				_rootCategory = Loader.Load(_openFileDialog.FileName, out loadedCultures, out lwt);
				IEditContext ec = (IEditContext)Site.GetService(typeof(IEditContext));
				ec.AvailableCultures = loadedCultures;
				_directory = Path.GetDirectoryName(_openFileDialog.FileName);
				FakeNodeCollection fnc = new FakeNodeCollection();
				fnc.Add(_rootCategory);
				_rootCategory.TreeParent = fnc;
				ClearSelection();
				_categoryTree.Nodes = fnc;
				EnableCommandItems();
			}
		}

		private void GenerateHelper()
		{
			using (GenParamsForm gpf = new GenParamsForm())
			{
				gpf.Namespace = _rootCategory.Name;
				if (gpf.ShowDialog(this) == DialogResult.OK)
				{
					CodeCompileUnit ccu = HelperGenerator.Generate(_rootCategory,
						gpf.Namespace, gpf.IsInternal);
					CSharpCodeProvider cscp = new CSharpCodeProvider();
					CodeGeneratorOptions cgo = new CodeGeneratorOptions();
					cgo.BracingStyle = "C";
					using (StreamWriter sw = new StreamWriter(
							Path.Combine(_directory, _rootCategory.TreeName + ".cs")))
						cscp.GenerateCodeFromCompileUnit(ccu, sw, cgo);
					if (gpf.ShowResult)
						using (ShowResultForm srf = new ShowResultForm())
						using (StringWriter strW = new StringWriter())
						{
							cscp.GenerateCodeFromCompileUnit(ccu, strW, cgo);
							srf.Result = strW.ToString();
							srf.ShowDialog(this);
						}
				}
			}
		}

		private void _exitMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void _openMenuItem_Click(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void _categoryTree_AfterActivateNode(ITreeNode ActivatedNode)
		{
			FillList();
		}

		private void FillList()
		{
			_itemList.BeginUpdate();
			try
			{
				_itemList.Items.Clear();
				if (_categoryTree.ActiveNode == null)
					return;
				Category c = (Category)_categoryTree.ActiveNode;
				ArrayList al = new ArrayList(c.ResourceItems);
				foreach (ResourceItem ri in al)
				{
					ListViewItem lvi = CreateListItem(ri);
					_itemList.Items.Add(lvi);
				}
			}
			finally
			{
				_itemNameCol.Width = -1;
				_itemList.EndUpdate();
			}
			if (_itemList.Items.Count > 0)
			{
				_itemList.Items[0].Focused = true;
				_itemList.Items[0].Selected = true;
			}
			else
				_propertyGrid.SelectedObject = null;
		}

		private static ListViewItem CreateListItem(ResourceItem ri)
		{
			ListViewItem lvi = new ListViewItem(ri.Name, 0);
			lvi.Tag = ri;
			return lvi;
		}

		private void _itemList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_itemList.FocusedItem != null)
				_propertyGrid.SelectedObject = _itemList.FocusedItem.Tag;
			else
				_propertyGrid.SelectedObject = null;
		}

		private void _saveMenuItem_Click(object sender, EventArgs e)
		{
			_rootCategory.Save(_directory);
		}

		private void _generateMenuItem_Click(object sender, EventArgs e)
		{
			GenerateHelper();
		}

		private void _addResourceMenuItem_Click(object sender, EventArgs e)
		{
			using (ResourceNameForm rnf = new ResourceNameForm())
			{
				Category cc = (Category)_categoryTree.ActiveNode;
				string prefix = cc != null ? cc.Name + "." : "";
				rnf.ResourceName = prefix;
				if (rnf.ShowDialog(this) == DialogResult.OK)
				{
					ResourceItem ri = _rootCategory.GetItem(rnf.ResourceName);
					using (new TreeUpdateHelper(_categoryTree))
						ri.ValueCollection[CultureInfo.InvariantCulture] = "value";
					ListViewItem lvi = CreateListItem(ri);
					using (new ListUpdateHelper(_itemList))
						_itemList.Items.Insert(0, lvi);
					_itemList.SelectedItems.Clear();
					lvi.Selected = lvi.Focused = true;
					_propertyGrid.Focus();
				}
			}
		}

		private void _removeResourceMenuItem_Click(object sender, EventArgs e)
		{
			if (_itemList.FocusedItem == null)
				return;
			ResourceItem ri = (ResourceItem)_itemList.FocusedItem.Tag;
			using (new TreeUpdateHelper(_categoryTree))
				ri.Delete();
			using (new ListUpdateHelper(_itemList))
				foreach (ListViewItem lvi in _itemList.Items)
					if (((ResourceItem)lvi.Tag).Name == ri.Name)
					{
						_itemList.Items.Remove(lvi);
						_propertyGrid.SelectedObject = null;
						break;
					}
		}

		private void _itemList_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (e.Label == null)
				return;
			ResourceItem ri = (ResourceItem)_itemList.Items[e.Item].Tag;
			Hashtable ht = new Hashtable();
			foreach (CultureInfo ci in ri.ValueCollection.GetCultures())
				ht[ci] = ri.ValueCollection[ci];
			using (new TreeUpdateHelper(_categoryTree))
			{
				ri.Delete();
				ResourceItem nri = _rootCategory.GetItem(e.Label);
				foreach (CultureInfo ci in ht.Keys)
					nri.ValueCollection[ci] = (string)ht[ci];
			}
			ClearSelection();
			e.CancelEdit = true;
		}

		private void ClearSelection()
		{
			Application.DoEvents();
			FillList();
			_propertyGrid.SelectedObject = null;
		}

		private void _renameResourceMenuItem_Click(object sender, EventArgs e)
		{
			if (_itemList.FocusedItem != null)
				_itemList.FocusedItem.BeginEdit();
		}

		private void _itemList_MouseUp(object sender, MouseEventArgs e)
		{
			if (_contextMenuEnabled && (e.Button & MouseButtons.Right) > 0)
				if (_itemList.FocusedItem != null)
					_listContextMenu.Show(_itemList, new Point(e.X, e.Y));
		}

		private void _categoryTree_MouseUp(object sender, MouseEventArgs e)
		{
			if (_contextMenuEnabled && (e.Button & MouseButtons.Right) > 0)
				if (_categoryTree.ActiveNode != null)
					_treeContextMenu.Show(_categoryTree, new Point(e.X, e.Y));
		}

		private void _categoryTree_GetData(object sender, GetDataEventArgs e)
		{
			Category cat = (Category)e.Node;
			RootCategory rCat = e.Node as RootCategory;
			if (rCat != null)
			{
				e.CellInfos[0].ImageKey = "root";
				e.CellInfos[0].Text = rCat.TreeName;
			}
			else
			{
				e.CellInfos[0].ImageKey = "category";
				e.CellInfos[0].Text = cat.ShortName;
			}

			e.CellInfos[1].Text = string.Format("{0}/{1}", cat.ResourceItems.Count, cat.GetAllResCount());
		}

		private void EnableCommandItems()
		{
			_saveButton.Enabled = _saveMenuItem.Enabled =
			_generateButton.Enabled = _generateMenuItem.Enabled =
			_removeResourceButton.Enabled = _addResourceMenuItem.Enabled =
			_addResourceButton.Enabled = _removeResourceMenuItem.Enabled =
			_renameResourceButton.Enabled = _renameResourceButton.Enabled =
			_resourceMenuItem.Enabled =
			_manageCulturesMenuItem.Enabled =
			_contextMenuEnabled = true;
		}

		private void _manageCulturesMenuItem_Click(object sender, EventArgs e)
		{
			IEditContext ec = (IEditContext)Site.GetService(typeof(IEditContext));
			using (CultureManagerForm cmf = new CultureManagerForm(ec.AvailableCultures))
				if (cmf.ShowDialog(ec.DialogsOwner) == DialogResult.OK)
				{
					ec.AvailableCultures = cmf.Cultures;
					_propertyGrid.Refresh();
				}
		}

		#region UpdateHelpers
		private class TreeUpdateHelper : IDisposable
		{
			private readonly TreeGrid.TreeGrid _treeGrid;
			private readonly ITreeNode _nodes;
			private readonly ITreeNode _activeNode;

			public TreeUpdateHelper(TreeGrid.TreeGrid treeGrid)
			{
				_treeGrid = treeGrid;
				_nodes = _treeGrid.Nodes;
				_activeNode = _treeGrid.ActiveNode;
				_treeGrid.Nodes = null;
			}

			public void Dispose()
			{
				_treeGrid.Nodes = _nodes;
				_treeGrid.ActiveNode = _activeNode;
			}
		}

		private class ListUpdateHelper : IDisposable
		{
			private readonly ListView _list;
			private readonly Dictionary<string, object> _selectedItems = new Dictionary<string, object>();
			private readonly string _focusedItem;

			public ListUpdateHelper(ListView list)
			{
				_list = list;
				foreach (ListViewItem lvi in list.SelectedItems)
					_selectedItems[((ResourceItem)lvi.Tag).Name] = null;
				if(list.FocusedItem != null)
					_focusedItem = ((ResourceItem)list.FocusedItem.Tag).Name;
				_list.BeginUpdate();
			}

			public void Dispose()
			{
				foreach (ListViewItem lvi in _list.Items)
				{
					ResourceItem ri = (ResourceItem)lvi.Tag;
					if (_selectedItems.ContainsKey(ri.Name))
						lvi.Selected = true;
					if (ri.Name == _focusedItem)
						lvi.Focused = true;
				}
				_list.EndUpdate();
			}
		}
		#endregion

		#region class FakeNodeCollection
		private class FakeNodeCollection : CollectionBase, ITreeNode
		{
			private NodeFlags _flags;

			public ITreeNode Parent
			{
				get { return null; }
			}

			public NodeFlags Flags
			{
				get { return _flags; }
				set { _flags = value; }
			}

			public bool HasChildren
			{
				get { return List.Count > 0; }
			}

			public ITreeNode this[int index]
			{
				get { return (ITreeNode)List[index]; }
			}

			public void Add(ITreeNode node)
			{
				List.Add(node);
			}
		}
		#endregion
	}
}
