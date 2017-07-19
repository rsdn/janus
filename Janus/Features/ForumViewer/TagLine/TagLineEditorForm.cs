using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Services;
using CodeJam.Strings;

using JetBrains.Annotations;

using Rsdn.Scintilla;

namespace Rsdn.Janus
{
	/// <summary>
	/// Форма редактора тег лайна.
	/// </summary>
	internal partial class TagLineEditorForm : Form
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly HashSet<int> _excludeHash;
		private readonly ITextMacrosService _textMacrosService;

		private string _tagLineFormatResult;

		public TagLineEditorForm([NotNull] IServiceProvider provider, [NotNull] HashSet<int> excludeHash)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));
			if (excludeHash == null)
				throw new ArgumentNullException(nameof(excludeHash));

			_serviceProvider = provider;
			_textMacrosService = _serviceProvider.GetService<ITextMacrosService>();
			_excludeHash = excludeHash;

			InitializeComponent();

			_allForumsCheck.Enabled = _allForumsCheck.Checked =
				!excludeHash.Contains(TagLineInfo.AllForums);

			_forumsImages.Images.Add(provider.GetRequiredService<IStyleImageManager>()
				.GetImage(@"NavTree\Forum", StyleImageType.ConstSize));

			FillForumsTree();

			FillMacrosMenu();
		}

		public string TagLineName
		{
			get { return _nameBox.Text; }
			set { _nameBox.Text = value; }
		}

		public string TagLineFormat
		{
			get { return _tagLineFormatResult ?? _formatEditor.Model.Text; }
			set { _formatEditor.Model.Text = value; }
		}

		public int[] Forums
		{
			get
			{
				if (_allForumsCheck.Checked)
					return new[] { TagLineInfo.AllForums };

				return 
					(from TreeNode tn in _forumsTree.Nodes
					where tn.Checked
					select (int)tn.Tag)
						.ToArray();
			}
			set
			{
				if (value.Length > 0 && value[0] == TagLineInfo.AllForums)
				{
					_allForumsCheck.Checked = true;
					foreach (TreeNode tn in _forumsTree.Nodes)
						tn.Checked = false;
				}
				else
				{
					_allForumsCheck.Checked = false;
					foreach (TreeNode tn in _forumsTree.Nodes)
						tn.Checked = Array.IndexOf(value, (int)tn.Tag) >= 0;
				}
			}
		}

		private void AllForumsCheckCheckedChanged(object sender, EventArgs e)
		{
			_forumsTree.Enabled = !_allForumsCheck.Checked;
		}

		private void FillForumsTree()
		{
			using (var dbMgr = _serviceProvider.CreateDBContext())
			using (_forumsTree.UpdateScope(true))
			{
				var forums =
					dbMgr
						.ServerForums()
						.Select(frm => new { frm.ID, frm.Name, frm.Descript });
				foreach (var f in forums)
				{
					if (_excludeHash.Contains(f.ID))
						continue;

					var tn =
						new TreeNode(ForumHelper.GetDisplayName(f.Name, f.Descript), 0, 0)
						{
							Tag = f.ID
						};

					_forumsTree.Nodes.Add(tn);
				}
			}
		}

		private void FormatEditorStyleNeeded(object sender, StyleNeededEventArgs e)
		{
			var matches = TextMacrosHelper.FindMacroses(_serviceProvider, _formatEditor.Model.Text);
			for (var i = e.StartPosition; i < e.EndPosition; i++)
			{
				_formatEditor.StartStyling(i, 31);

				int length;
				if (matches.TryGetValue(i, out length))
				{
					_formatEditor.SetStyling(length, _macroStyle);
					i += length - 1;
				}
				else
					_formatEditor.SetStyling(1, _defaultStyle);
			}
		}

		private void TagLineEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
				_tagLineFormatResult = _formatEditor.Model.Text;
		}

		private void MakeDefaultButtonClick(object sender, EventArgs e)
		{
			TagLineFormat = TagLineInfo.DefaultInfo.Format;
		}

		private void FillMacrosMenu()
		{
			_macrosContextMenuStrip.Items.Clear();

			if (_textMacrosService == null || !_textMacrosService.TextMacroses.Any())
			{
				_macrosButton.Enabled = false;
				return;
			}

			_macrosContextMenuStrip.Items.AddRange(
				_textMacrosService
					.TextMacroses
					.Select(
						macros => new ToolStripMenuItem(
							"@@{0} - {1}".FormatWith(macros.MacrosText, macros.DisplayName),
							null,
							MacrosMenuItemClick) { Tag = macros })
					.ToArray());
		}

		private void MacrosMenuItemClick(object sender, EventArgs e)
		{
			_formatEditor.Selection.Text = "@@" + ((ITextMacros)((ToolStripMenuItem)sender).Tag).MacrosText;
		}

		private void _formatEditor_CharAdded(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '@'
					&& _formatEditor.Model.CharAt(_formatEditor.CaretPosition - 2) == '@'
					&& _textMacrosService != null
					&& _textMacrosService.TextMacroses.Any())
				_formatEditor.ShowAutocomplete(
					2,
					_serviceProvider
						.GetRequiredService<ITextMacrosService>()
						.TextMacroses
						.Select(macros => macros.MacrosText)
						.OrderBy(name => name)
						.Select(name => "@@" + name));
		}

		private void _macrosButton_MouseDown(object sender, MouseEventArgs e)
		{
			_macrosContextMenuStrip.Show(_macrosButton, e.Location);
		}
	}
}