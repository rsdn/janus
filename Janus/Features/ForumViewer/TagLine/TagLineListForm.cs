using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Главная форма редактора тег-лайнов.
	/// </summary>
	internal sealed partial class TagLineListForm : Form
	{
		private readonly ServiceContainer _serviceManager;
		private readonly StripMenuGenerator _toolbarGenerator;
		private readonly StripMenuGenerator _contextMenuGenerator;
		private readonly ObservableList<TagLineInfo> _tagLines;

		public event SelectedTagLinesChangedEventHandler SelectedTagLinesChanged;

		public TagLineListForm(IServiceProvider provider, IEnumerable<TagLineInfo> tagLines)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));
			if (tagLines == null)
				throw new ArgumentNullException(nameof(tagLines));

			_serviceManager = new ServiceContainer(provider);

			_tagLines = new ObservableList<TagLineInfo>(tagLines);

			InitializeComponent();

			_serviceManager.Publish<ITagLineListFormService>(new TagLineListFormService(this));
			_serviceManager.Publish<IDefaultCommandService>(new DefaultCommandService("Janus.Forum.TagLine.Edit"));

			_toolbarGenerator = new StripMenuGenerator(_serviceManager, _toolStrip, "Forum.TagLine.Toolbar");
			_contextMenuGenerator = new StripMenuGenerator(_serviceManager, _contextMenuStrip, "Forum.TagLine.ContextMenu");

			_listImages.Images.Add(
				_serviceManager.GetRequiredService<IStyleImageManager>()
					.GetImage(@"MessageTree\Msg", StyleImageType.ConstSize));

			UpdateData();

			_tagLines.Changed += (sender, e) => UpdateData();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_toolbarGenerator.Dispose();
				_contextMenuGenerator.Dispose();
				components?.Dispose();
			}
			base.Dispose(disposing);
		}

		public ObservableList<TagLineInfo> TagLines => _tagLines;

		public IEnumerable<TagLineInfo> SelectedTagLines
		{
			get
			{
				return _tagsList
					.SelectedItems
					.Cast<ListViewItem>()
					.Select(item => (TagLineInfo)item.Tag)
					.ToArray();
			}
		}

		private void OnSelectedTagLinesChanged()
		{
			SelectedTagLinesChanged?.Invoke(this);
		}

		private void UpdateData()
		{
			using (_tagsList.UpdateScope(true))
				foreach (var tgi in _tagLines)
					_tagsList.Items.Add(
						new ListViewItem(new[] {tgi.Name, tgi.Format}, 0) {Tag = tgi});
		}

		private void _tagsList_DoubleClick(object sender, EventArgs e)
		{
			_serviceManager.ExecuteDefaultCommand();
		}

		private void _tagsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnSelectedTagLinesChanged();
		}
	}
}