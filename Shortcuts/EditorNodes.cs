using System;
using System.Windows.Forms;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Summary description for EditorNodes.
	/// </summary>
	internal partial class EditorNodes : Form
	{
		internal ShortcutCollection _collection;

		private int _nextNode;

		public EditorNodes()
		{
			InitializeComponent();
		}

		private static Type GetType(string typeName)
		{
			try
			{
				return Type.GetType(typeName, true);
			}
			catch
			{
				MessageBox.Show(
					string.Format("Тип '{0}' не найден, установлен 'System.Type'",
						typeName),
					"Ошибка");

				return typeof (Type);
			}
		}

		private void CreateCollection()
		{
			_collection.Clear();
			var node = _root;

			var cs = new CustomShortcut(GetType((string)node.Tag), node.Text);

			_collection.Add(cs);
			FillNode(cs, node.Nodes);
		}

		private static void FillNode(CustomShortcut shortcut, TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				var cs = new CustomShortcut(
					GetType((string)node.Tag), node.Text);

				shortcut.Nodes.Add(cs);
				FillNode(cs, node.Nodes);
			}
		}

		private static void BuildTree(TreeNode currentNode, ShortcutCollection node)
		{
			if (node == null)
				return;

			foreach (CustomShortcut cs in node)
			{
				var treeNode = new TreeNode(cs.Name) {Tag = cs.OwnerType.ToString()};

				currentNode.Nodes.Add(treeNode);

				BuildTree(treeNode, cs.Nodes);
			}
		}

		private void EditorNodes_Load(object sender, EventArgs e)
		{
			_nextNode = 0;

			if (_collection.Count > 0)
			{
				var cs = _collection[0];

				_root = new TreeNode(cs.Name) {Tag = cs.OwnerType.ToString()};

				_treeView.Nodes.Add(_root);

				BuildTree(_root, cs.Nodes);
			}
		}

		private void _addButton_Click(object sender, EventArgs e)
		{
			var node = _currentNode.Nodes.Add("node" + _nextNode);

			_currentNode.Expand();

			_treeView.SelectedNode = node;

			_nextNode++;
		}

		private void _deleteButton_Click(object sender, EventArgs e)
		{
			if (_currentNode != _root)
				_currentNode.Remove();
		}

		private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			_currentNode = e.Node;
			_descriptionBox.Text = _currentNode.Text;
			_nameBox.Text = (string)_currentNode.Tag;
		}

		private void _okButton_Click(object sender, EventArgs e)
		{
			CreateCollection();
			Close();
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void _descriptionBox_TextChanged(object sender, EventArgs e)
		{
			_currentNode.Text = _descriptionBox.Text;
		}

		private void _nameBox_TextChanged(object sender, EventArgs e)
		{
			_currentNode.Tag = _nameBox.Text;
		}
	}
}