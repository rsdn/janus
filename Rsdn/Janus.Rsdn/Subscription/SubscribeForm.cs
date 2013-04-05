using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using JetBrains.Annotations;

using LinqToDB;

using Rsdn.Janus.AT;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Форма подписки на форумы
	/// </summary>
	public partial class SubscribeForm : JanusBaseForm
	{
		private readonly IServiceProvider _provider;
		private List<ForumData> _forumList;

		#region Constructor(s) & Dispose
		[Obsolete]
		public SubscribeForm()
		{ }

		public SubscribeForm([NotNull] IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			_provider = provider;

			InitializeComponent();
			CustomInitializeComponent();
		}

		#endregion

		#region Синхронизация списка форумов с сервером
		private void SyncForums()
		{
			_provider.SyncForums(
				_provider.GetRequiredService<IRsdnSyncConfigService>().GetConfig().SyncThreadPriority,
				(syncPerformed, stats) =>
				{
					if (syncPerformed)
						RefreshForm();
				},
				true);
		}
	
		#endregion

		private const string _sortByDesc = "3";
		private const string _sortByName = "2";
		private const string _sortByPriorityAndDesc = "5 DESC, 3";
		private const string _sortByPriorityAndName = "5 DESC, 2";

		private string _sortBy;
		private SortType _sortType;

		private void CustomInitializeComponent()
		{
			var cfg = _provider.GetRequiredService<IRsdnForumService>().GetConfig();
			_sortBy =
				cfg.ShowFullForumNames
					? _sortByDesc
					: _sortByName;
			_sortType =
				cfg.ShowFullForumNames
					? SortType.ByDesc
					: SortType.ByName;
			InitListView(false);
		}

		private void InitListView(bool preserveCheckStates)
		{
			var currentCheckStates = new Dictionary<string, bool>();
			if (preserveCheckStates)
				foreach (ListViewItem lvItem in _forumListView.Items)
					currentCheckStates[lvItem.SubItems[1].Text] = lvItem.Checked;

			using (var db = _provider.CreateDBContext())
			{
				db
					.SubscribedForums(f => f.Priority == null)
						.Set(_ => _.Priority, 0)
					.Update();

				var forums =
					ForumsSubscriptionHelper.GetAllForums(
						db,
						f => new { f.ID, f.Name, f.Descript, Priority = 0, Subscribed = false },
						f => new { f.ID, f.Name, f.Descript, Priority = f.Priority ?? 0, Subscribed = true });
				switch (_sortType)
				{
					case SortType.ByDesc:
						forums = forums.OrderBy(f => f.Descript);
						break;
					case SortType.ByName:
						forums = forums.OrderBy(f => f.Name);
						break;
					case SortType.ByPriorityAndName:
						forums = forums.OrderBy(f => f.Priority).ThenBy(f => f.Name);
						break;
					case SortType.ByPriorityAndDesc:
						forums = forums.OrderBy(f => f.Priority).ThenBy(f => f.Descript);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				_forumList =
					forums
						.Select(f => new ForumData(f.ID, f.Name, f.Descript, f.Priority, f.Subscribed))
						.ToList();
			}

			using (_forumListView.UpdateScope(true))
				for (var i = 0; i < _forumList.Count; i++)
				{
					var forum = _forumList[i];

					var lvi = new ListViewItem(
						new[]
						{
							forum.Priority.ToString(),
							forum.Name,
							forum.Descript
						})
					{
						Checked = forum.Subscribed,
						ImageIndex = 0,
						Tag = i
					};

					// restore user changes
					if (currentCheckStates.ContainsKey(forum.Name))
						lvi.Checked = currentCheckStates[forum.Name];

					_forumListView.Items.Add(lvi);
				}

			if (_forumList.Count > 0)
				_priority.Value = _forumList[0].Priority;
		}

		private void BtnApplyClick(object sender, EventArgs e)
		{
			Apply();
		}

		private void BtnOkClick(object sender, EventArgs e)
		{
			Apply();
		}

		private void Apply()
		{
			if (_forumListView.FocusedItem != null)
				_forumListView.FocusedItem.SubItems[(int)Column.Priority].Text =
					_priority.Value.ToString();

			var subscriptionRequests = new List<ForumSubscriptionRequest>();
			foreach (ListViewItem item in _forumListView.Items)
			{
				var forum = _forumList[(int)item.Tag];

				if (forum.Subscribed != item.Checked)
					subscriptionRequests.Add(new ForumSubscriptionRequest(forum.ID, item.Checked));

				var newPriority = int.Parse(item.SubItems[0].Text);

				if (item.Checked && forum.Priority != newPriority)
					ForumsSubscriptionHelper.UpdateForumPriority(_provider, forum.ID, newPriority);
			}

			if (subscriptionRequests.Count > 0)
				ForumsSubscriptionHelper.UpdateForumsSubscriptions(_provider, subscriptionRequests, true);

			//без рефреша возникало исключение если нажать "применить" потом "ок"
			RefreshForm();
		}

		private void BtnSyncForumsClick(object sender, EventArgs e)
		{
			SyncForums();
		}

		private void RefreshForm()
		{
			InitListView(true);
		}

		private void ForumListViewColumnClick(object sender, ColumnClickEventArgs e)
		{
			switch ((Column)e.Column)
			{
				case Column.Priority:
					_sortBy = 
						_sortBy == _sortByDesc || _sortBy == _sortByPriorityAndDesc
							? _sortByPriorityAndDesc
							: _sortByPriorityAndName;
					_sortType =
						_sortType == SortType.ByDesc || _sortType == SortType.ByPriorityAndDesc
							? SortType.ByPriorityAndDesc
							: SortType.ByPriorityAndName;
					break;
				case Column.Name:
					_sortType = SortType.ByName;
					break;
				case Column.Desc:
					_sortType = SortType.ByDesc;
					break;
				default:
					Trace.Assert(false,
						"Нажатие на колонку с неизвесным ID (" + e.Column + ")");
					break;
			}

			InitListView(true);
		}

		private void ForumListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			if (_forumListView.FocusedItem == null || !_forumListView.FocusedItem.Checked)
			{
				_priority.Enabled = false;
				_priority.Value = 0;
			}
			else
			{
				var value = _forumListView.FocusedItem.SubItems[0].Text;

				_priority.Value = int.Parse(value);
				_priority.Enabled = true;
			}
		}

		private void PriorityValueChanged(object sender, EventArgs e)
		{
			if (_forumListView.FocusedItem == null)
				return;

			_forumListView.FocusedItem.SubItems[(int)Column.Priority].Text =
				_priority.Value.ToString();
		}

		#region Nested type: Column
		private enum Column
		{
			Priority,
			Name,
			Desc,
		}
		#endregion

		#region SortType
		private enum SortType
		{
			ByDesc,
			ByName,
			ByPriorityAndName,
			ByPriorityAndDesc
		}
		#endregion

		#region ForumData class
		private class ForumData
		{
			private readonly int _id;
			private readonly string _name;
			private readonly string _descript;
			private readonly int _priority;
			private readonly bool _subscribed;

			public ForumData(
				int id,
				string name,
				string descript,
				int priority,
				bool subscribed)
			{
				_id = id;
				_name = name;
				_descript = descript;
				_priority = priority;
				_subscribed = subscribed;
			}

			public int ID
			{
				get { return _id; }
			}

			public string Name
			{
				get { return _name; }
			}

			public string Descript
			{
				get { return _descript; }
			}

			public int Priority
			{
				get { return _priority; }
			}

			public bool Subscribed
			{
				get { return _subscribed; }
			}
		}
		#endregion
	}
}