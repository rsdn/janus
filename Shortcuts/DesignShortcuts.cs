using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Summary description for DesignShortcut.
	/// </summary>
	public partial class DesignShortcuts : Form, IDialogContainer
	{
		#region  Class Variables
		private readonly ShortcutManager _manager;
		private CustomShortcut _shortcut;
		#endregion

		#region Constructor
		public DesignShortcuts(ShortcutManager manager)
		{
			_manager = manager;

			InitializeComponent();
			CustomInitializeComponent();
		}
		#endregion

		#region Implementation
		public Control GetDialog()
		{
			return _containerPanel;
		}

		public void AcceptChanges()
		{
			ReFillHashTable();
			//ShortcutManager.Instance.Tree = tree;
			Close();
			//Hide();
		}

		public void RejectChanges()
		{
			Close();
		}

		private void CustomInitializeComponent()
		{
			LoadShortcuts();
		}

		private void LoadShortcuts()
		{
			try
			{
				_treeView.BeginUpdate();
				_treeView.Nodes.Clear();
				BuildTree(_treeView.Nodes, _manager.Nodes);
			}
			finally
			{
				_treeView.EndUpdate();
			}

			_shortcut = null;

			if (_treeView.Nodes.Count > 0)
			{
				_treeView.SelectedNode = _treeView.Nodes[0];
				_treeView.ExpandAll();
			}
		}

		private static void BuildTree(TreeNodeCollection treeNodes,
			ShortcutCollection shortcutNodes)
		{
			foreach (CustomShortcut shortcut in shortcutNodes)
			{
				var node = new TreeNode(shortcut.Name, 0, 1) {Tag = shortcut};

				treeNodes.Add(node);

				BuildTree(node.Nodes, shortcut.Nodes);
			}
		}

		private void ReFillHashTable()
		{
			if (_listBox.Items.Count != 0 && _shortcut != null)
			{
				_shortcut.HashTable.Clear();

				foreach (TextShortcut ts in _listBox.Items)
					if (ts.Shortcut != Shortcut.None)
						_shortcut.HashTable[ts.Shortcut] = ts.MethodName;
			}
		}

		private void FillListBox(bool refresh)
		{
			_listBox.Items.Clear();

			var shortcuts =
				new Dictionary<string, Shortcut>();

			foreach (var sc in _shortcut.HashTable)
				shortcuts[sc.Value] = sc.Key;

			var bindingAttr =
				BindingFlags.DeclaredOnly
					| BindingFlags.Instance
						| BindingFlags.Static
							| BindingFlags.Public
								| BindingFlags.NonPublic;


			foreach (var mi in _shortcut.OwnerType.GetMethods(bindingAttr))
			{
				var attr = (MethodShortcutAttribute)Attribute
					.GetCustomAttribute(mi, typeof (MethodShortcutAttribute));

				if (attr != null)
					if (refresh)
						_listBox.Items.Add(new TextShortcut(
							mi.Name, attr.ShortName, attr.LongName, attr.Shortcut));
					else
					{
						Shortcut sc;
						if (!shortcuts.TryGetValue(mi.Name, out sc))
							sc = Shortcut.None;

						_listBox.Items.Add(new TextShortcut(
							mi.Name, attr.ShortName, attr.LongName, sc));
					}
			}
		}

		private void ProcessToolbarClick(string action)
		{
			switch (action)
			{
				case "SaveAsPreset":
					SaveAsPreset();
					break;
				case "LoadPreset":
					LoadPreset();
					break;
			}
		}

		private void SaveAsPreset()
		{
			ReFillHashTable();

			using (var spf = new SavePresetForm())
				if (spf.ShowDialog() == DialogResult.OK)
					_manager.SaveCurrentAsPreset(spf.PresetName);
		}

		private void LoadPreset()
		{
			using (var spf = new SelectPresetForm(_manager))
			{
				spf.LoadPresets(_manager.Presets);
				if (spf.ShowDialog() == DialogResult.OK)
				{
					_manager.LoadPreset(spf.PresetName);
					LoadShortcuts();
					//FillListBox(true);
					FillListBox(false);
				}
			}
		}
		#endregion

		#region Events
		private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			_shortNameLabel.Text = string.Empty;
			_longNameLabel.Text = string.Empty;

			ReFillHashTable();
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			_shortcut = (CustomShortcut)e.Node.Tag;
			FillListBox(false);
		}

		private void _okButton_Click(object sender, EventArgs e)
		{
			AcceptChanges();
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			Close();
			//Hide();
		}

		private void _refreshButton_Click(object sender, EventArgs e)
		{
			FillListBox(true);
		}

		private void listBox1_SelectedValueChanged(object sender, EventArgs e)
		{
			if (_listBox.SelectedItem != null)
			{
				var ts = (TextShortcut)_listBox.SelectedItem;

				_shortNameLabel.Text = ts.ShortName;
				_longNameLabel.Text = ts.LongName;
			}
		}

		private void _applyButton_Click(object sender, EventArgs e)
		{
			ReFillHashTable();
			//ShortcutManager.Instance.Tree = tree;
		}

		private void _toolbar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			ProcessToolbarClick(e.Button.Tag.ToString());
		}
		#endregion
	}
}