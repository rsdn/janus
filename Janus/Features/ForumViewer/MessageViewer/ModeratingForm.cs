using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal partial class ModeratingForm : JanusBaseForm
	{
		private readonly ImageList _images = new ImageList();
		private readonly int _mdrDelImage;
		private readonly int _mdrExtractImage;
		private readonly int _mdrLeaveImage;
		private readonly int _mdrMoveImage;
		private readonly int _mdrCloseTopicImage;
		private readonly int _mdrOpenTopicImage;
		private readonly ModInfo[] _moderatorials;
		private readonly DateTime? _lastModerated;
		private readonly int _forumID;

		[Obsolete]
		public ModeratingForm()
		{ }

		public ModeratingForm([NotNull] IServiceProvider provider, int msgId)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			var styleMgr = provider.GetRequiredService<IStyleImageManager>();
			_mdrExtractImage = styleMgr.AppendImage(
				@"MessageViewer\MdrExtract",
				StyleImageType.ConstSize,
				_images);
			_mdrMoveImage = styleMgr.AppendImage(
				@"MessageViewer\MdrMove",
				StyleImageType.ConstSize,
				_images);
			_mdrDelImage = styleMgr.AppendImage(
				"del",
				StyleImageType.Small,
				_images);
			_mdrLeaveImage = styleMgr.AppendImage(
				@"MessageViewer\MdrLeave",
				StyleImageType.ConstSize,
				_images);
			_mdrCloseTopicImage = styleMgr.AppendImage(
				@"MessageViewer\MdrCloseTopic",
				StyleImageType.ConstSize,
				_images);
			_mdrOpenTopicImage = styleMgr.AppendImage(
				@"MessageViewer\MdrOpenTopic",
				StyleImageType.ConstSize,
				_images);

			//_msg = DatabaseManager.GetMessagesList(new[] { msgId })[0];
			InitializeComponent();

			_moderatorialsList.SmallImageList = _images;

			using (var db = provider.CreateDBContext())
			{
				var msg = db.Message(msgId, m => new {m.ForumID, m.LastModerated});
				_forumID = msg.ForumID;
				_lastModerated = msg.LastModerated;
				_moderatorials =
					db
						.Moderatorials()
						.Where(m => m.MessageID == msgId)
						.OrderByDescending(m => m.Create)
						.Select(
							m =>
								new ModInfo(
									m.ForumID,
									JanusFormatMessage.GetModeratorialActionName(
										m.ForumID,
										_forumID,
										m.ServerForum.Name,
										m.ServerForum.Descript),
									m.User.DisplayName(),
									m.Create))
						.ToArray();
			}
			_moderatorialsList.VirtualListSize = _moderatorials.Length;
			_userCol.Width = _actionCol.Width = _createCol.Width = -1;
		}

		private int GetModImageIndex(int forumId, int msgForumId)
		{
			switch (forumId)
			{
				case -7:
					return _mdrCloseTopicImage;
				case -8:
					return _mdrOpenTopicImage;
				case -2:
					return _mdrExtractImage;
				case 0:
					return _mdrDelImage;
				default:
					return forumId != msgForumId ? _mdrMoveImage : _mdrLeaveImage;
			}
		}

		private void ModeratorialsList_RetrieveVirtualItem(
			object sender,
			RetrieveVirtualItemEventArgs e)
		{
			var mod = _moderatorials[e.ItemIndex];
			e.Item = new ListViewItem(
				new[]
				{
					mod.UserName,
					mod.ActionDesc,
					JanusFormatMessage.GetDateString(mod.Create)
				})
			{
				BackColor =
					_lastModerated == null || mod.Create > _lastModerated
						? SystemColors.Window
						: SystemColors.ControlDark,
				ImageIndex = GetModImageIndex(mod.ForumId, _forumID)
			};
		}

		#region class ModInfo
		private class ModInfo
		{
			private readonly int _forumId;
			private readonly string _actionDesc;
			private readonly string _userName;
			private readonly DateTime _create;

			public ModInfo(
				int forumId,
				string actionDesc,
				string userName,
				DateTime create)
			{
				_forumId = forumId;
				_actionDesc = actionDesc;
				_userName = userName;
				_create = create;
			}

			public int ForumId
			{
				get { return _forumId; }
			}

			public string ActionDesc
			{
				get { return _actionDesc; }
			}

			public string UserName
			{
				get { return _userName; }
			}

			public DateTime Create
			{
				get { return _create; }
			}
		}
		#endregion
	}
}