using System;
using System.Windows.Forms;
using System.Linq;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for ForumsBox.
	/// </summary>
	public class ForumsBox : ComboBox
	{
		private readonly IServiceProvider _provider;

		#region ForumDescrContainer (Item container)

		private class ForumDescrContainer
		{
			public ForumDescrContainer(string name, int id)
			{
				_name = name;
				_id = id;
			}

			private readonly string _name;

			private readonly int _id;
			public int Id
			{
				get { return _id; }
			}

			public override string ToString()
			{
				return _name ?? string.Empty;
			}
		}

		#endregion

		public ForumsBox(IServiceProvider provider)
		{
			_provider = provider;
			_forumId = -1;

			InitializeComponent();

			DropDownStyle = ComboBoxStyle.DropDownList;
			MaxDropDownItems = 12;
			Items.Clear();
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			// 
			// ForumsBox
			// 
			SelectedIndexChanged += ForumsBox_SelectedIndexChanged;
			ResumeLayout(false);
		}

		public void InitForumsComboBox(string desc, int selectedId)
		{
			// add top item
			Items.Add(new ForumDescrContainer(desc, -1));
			if (selectedId == -1)
				SelectedIndex = 0;

			InitForumsComboBox(selectedId);
		}

		public void InitForumsComboBox(int selectedId)
		{
			// fill combobox
			using (var mgr = _provider.CreateDBContext())
			{
				var fullNames = Config.Instance.ForumDisplayConfig.ShowFullForumNames;
				var forums =
					mgr
						.SubscribedForums()
						.OrderBy(f => fullNames ? f.Descript : f.Name)
						.Select(f =>
							new ForumDescrContainer(
								fullNames ? f.Descript : f.Name,
								f.ID))
						.ToArray();
				Items.AddRange(forums);
				var sel =
					forums
						.Select((f, i) => new {f, i})
						.FirstOrDefault(p => p.f.Id == selectedId);
				if (sel != null)
					SelectedIndex = sel.i;
			}
		}

		private int _forumId;
		public int ForumId
		{
			get { return _forumId; }
		}

		private void ForumsBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selItem = SelectedItem;
			_forumId = selItem == null ? -1 : ((ForumDescrContainer)selItem).Id;
		}
	}
}
