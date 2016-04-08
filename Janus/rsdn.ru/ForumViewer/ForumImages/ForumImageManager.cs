using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using CodeJam.Collections;
using CodeJam.Extensibility;
using CodeJam.Services;

namespace Rsdn.Janus
{
	[Service(typeof (IForumImageManager))]
	internal class ForumImageManager : IForumImageManager
	{
		private readonly Image _msgImage;
		private readonly Image _msgUnreadImage;
		private readonly Image _msgArticleImage;
		private readonly Image _msgArticleUnreadImage;
		private readonly Image _mdrImage;
		private readonly Image _mdrRepliesImage;
		private readonly Image _closedImage;
		private readonly Image _unreadRepliesToMeImage;

		private readonly ILazyDictionary<MsgImageKey, Image> _images;

		private readonly Image[] _dayOfWeekImages = new Image[7];
		private readonly Image[] _outdatedDayOfWeekImages = new Image[7];

		private static readonly Dictionary<UserClass, Image> _userImages =
			new Dictionary<UserClass, Image>();

		private readonly Image _markImage;
		private readonly Image _marksImage;
		private readonly Image _autoReadImage;

		private readonly Image _forumImage;
		private readonly Image _forumUnreadImage;
		private readonly Image _inTopImage;
		private readonly ILazyDictionary<ForumImageKey, Image> _forumImages;

		public ForumImageManager(IServiceProvider provider)
		{
			var styler = provider.GetRequiredService<IStyleImageManager>();
			const string prefix = @"MessageTree\";

			_msgImage = styler.GetImage(prefix + "Msg", StyleImageType.ConstSize);
			_msgUnreadImage = styler.GetImage(prefix + "MsgUnread", StyleImageType.ConstSize);
			_msgArticleImage = styler.GetImage(prefix + "MsgArticle", StyleImageType.ConstSize);
			_msgArticleUnreadImage = styler.GetImage(prefix + "MsgArticleUnread", StyleImageType.ConstSize);

			_mdrImage = styler.GetImage(prefix + "Moderatorial", StyleImageType.ConstSize);
			_mdrRepliesImage = styler.GetImage(prefix + "Moderatorials", StyleImageType.ConstSize);

			_closedImage = styler.GetImage(prefix + "Closed", StyleImageType.ConstSize);
			_unreadRepliesToMeImage = styler.GetImage(prefix + "UnreadRepliesToMe", StyleImageType.ConstSize);

			for (var i = 0; i < 7; i++)
			{
				var si = i.ToString();
				_dayOfWeekImages[i] = styler.GetImage(prefix + "WD" + si, StyleImageType.ConstSize);
				_outdatedDayOfWeekImages[i] = styler.GetImage(prefix + "WDOUT" + si, StyleImageType.ConstSize);
			}

			//иконки для классов пользователей
			_userImages[UserClass.User] = styler.GetImage(prefix + "User", StyleImageType.ConstSize);
			_userImages[UserClass.Anonym] = styler.GetImage(prefix + "UserAnonym", StyleImageType.ConstSize);
			_userImages[UserClass.Team] = styler.GetImage(prefix + "UserTeam", StyleImageType.ConstSize);
			_userImages[UserClass.Moderator] = styler.GetImage(prefix + "UserModerator",
				StyleImageType.ConstSize);
			_userImages[UserClass.Admin] = styler.GetImage(prefix + "UserAdmin", StyleImageType.ConstSize);
			_userImages[UserClass.Expert] = styler.GetImage(prefix + "UserExpert", StyleImageType.ConstSize);
			_userImages[UserClass.Group] = styler.GetImage(prefix + "UserGroup", StyleImageType.ConstSize);

			_markImage = styler.GetImage(prefix + "Mark", StyleImageType.ConstSize);
			_marksImage = styler.GetImage(prefix + "Marks", StyleImageType.ConstSize);

			_autoReadImage = styler.GetImage(prefix + "ReadReplies", StyleImageType.ConstSize);

			const string navPrefix = @"NavTree\";
			_forumImage = styler.GetImage(navPrefix + "Forum", StyleImageType.ConstSize);
			_forumUnreadImage = styler.GetImage(navPrefix + "ForumUnread", StyleImageType.ConstSize);
			_inTopImage = styler.GetImage(navPrefix + "InTopSign", StyleImageType.ConstSize);

			_images = LazyDictionary.Create<MsgImageKey, Image>(CreateMsgImage, true);
			_forumImages = LazyDictionary.Create<ForumImageKey, Image>(CreateForumImage, true);
		}

		private Image CreateMsgImage(MsgImageKey key)
		{
			var baseImage =
				key.Type == MessageType.Ordinal
					? key.HasUnreaded == MessageFlagExistence.None
						? _msgImage
						: _msgUnreadImage
					: key.HasUnreaded == MessageFlagExistence.None
						? _msgArticleImage
						: _msgArticleUnreadImage;
			if (!key.HasUnreadRepliesToMe && key.HasModeratorials == MessageFlagExistence.None && !key.Closed)
				return baseImage;
			var bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
			using (var grp = Graphics.FromImage(bmp))
			{
				grp.CompositingMode = CompositingMode.SourceOver;
				grp.CompositingQuality = CompositingQuality.HighQuality;
				grp.DrawImage(baseImage, 0, 0, 16, 16);

				if (key.Closed)
					grp.DrawImage(_closedImage, 0, 0, 16, 16);

				if (key.HasModeratorials != MessageFlagExistence.None)
				{
					var img =
						key.HasModeratorials == MessageFlagExistence.OnMessage
							? _mdrImage
							: _mdrRepliesImage;
					grp.DrawImage(img, 0, 0, 16, 16);
				}

				if (key.HasUnreadRepliesToMe)
					grp.DrawImage(_unreadRepliesToMeImage, 0, 0, 16, 16);
			}
			return bmp;
		}

		private Image GetMsgDateImage(int index, bool outdated)
		{
			if (index < 0 || index > 6)
				throw new ArgumentOutOfRangeException(nameof(index));
			return (outdated ? _outdatedDayOfWeekImages : _dayOfWeekImages)[index];
		}

		private Image CreateForumImage(ForumImageKey key)
		{
			var baseImage = key.HasUnread ? _forumUnreadImage : _forumImage;
			if (!key.InTop && !key.HasUnreadRepliesToMe)
				return baseImage;
			var bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
			using (var grp = Graphics.FromImage(bmp))
			{
				grp.CompositingMode = CompositingMode.SourceOver;
				grp.CompositingQuality = CompositingQuality.HighQuality;
				grp.DrawImage(baseImage, 0, 0, 16, 16);

				if (key.InTop)
					grp.DrawImage(_inTopImage, 0, 0, 16, 16);
				if (key.HasUnreadRepliesToMe)
					grp.DrawImage(_unreadRepliesToMeImage, 0, 0, 16, 16);
			}
			return bmp;
		}

		#region IForumImageManager Members
		public Image GetMessageImage(
			MessageType type,
			MessageFlagExistence hasUnreaded,
			bool hasUnreadRepliesToMe,
			MessageFlagExistence hasModeratorials,
			bool closed)
		{
			return
				_images[new MsgImageKey(type, hasUnreaded, hasUnreadRepliesToMe, hasModeratorials, closed)];
		}

		private static bool IsMsgOutdated(DateTime date)
		{
			var daysToOutdate = Config.Instance.ForumDisplayConfig.DaysToOutdate;
			return DateTime.Now.AddDays(-daysToOutdate) > date && daysToOutdate != 0;
		}

		public Image GetMessageDateImage(DateTime date)
		{
			return GetMsgDateImage((int)date.DayOfWeek, IsMsgOutdated(date));
		}

		public Image GetUserImage(UserClass userClass)
		{
			return _userImages[userClass];
		}

		public Image GetMarkImage(MessageFlagExistence existence)
		{
			switch (existence)
			{
				case MessageFlagExistence.None:
					return null;
				case MessageFlagExistence.OnMessage:
					return _markImage;
				case MessageFlagExistence.OnChildren:
					return _marksImage;
				default:
					throw new ArgumentOutOfRangeException(nameof(existence));
			}
		}

		public Image GetAutoReadImage(MessageFlagExistence existence)
		{
			return existence == MessageFlagExistence.None ? null : _autoReadImage;
		}

		public Image GetForumImage(bool hasUnread, bool inTop, bool hasUnreadRepliesToMe)
		{
			return _forumImages[new ForumImageKey(hasUnread, inTop, hasUnreadRepliesToMe)];
		}
		#endregion

		#region MsgImageKey struct
		private struct MsgImageKey : IEquatable<MsgImageKey>
		{
			private readonly MessageType _type;
			private readonly MessageFlagExistence _hasUnreaded;
			private readonly bool _hasUnreadRepliesToMe;
			private readonly MessageFlagExistence _hasModeratorials;
			private readonly bool _closed;

			public MsgImageKey(
				MessageType type,
				MessageFlagExistence hasUnreaded,
				bool hasUnreadRepliesToMe,
				MessageFlagExistence hasModeratorials,
				bool closed)
			{
				_type = type;
				_hasUnreaded = hasUnreaded;
				_hasUnreadRepliesToMe = hasUnreadRepliesToMe;
				_hasModeratorials = hasModeratorials;
				_closed = closed;
			}

			public MessageType Type => _type;

			public MessageFlagExistence HasUnreaded => _hasUnreaded;

			public bool HasUnreadRepliesToMe => _hasUnreadRepliesToMe;

			public MessageFlagExistence HasModeratorials => _hasModeratorials;

			public bool Closed => _closed;


			public static bool operator !=(MsgImageKey msgImageKey1, MsgImageKey msgImageKey2)
			{
				return !msgImageKey1.Equals(msgImageKey2);
			}

			public static bool operator ==(MsgImageKey msgImageKey1, MsgImageKey msgImageKey2)
			{
				return msgImageKey1.Equals(msgImageKey2);
			}

			public bool Equals(MsgImageKey msgImageKey)
			{
				if (!Equals(_type, msgImageKey._type))
					return false;
				if (!Equals(_hasUnreaded, msgImageKey._hasUnreaded))
					return false;
				if (!Equals(_hasUnreadRepliesToMe, msgImageKey._hasUnreadRepliesToMe))
					return false;
				if (!Equals(_hasModeratorials, msgImageKey._hasModeratorials))
					return false;
				if (!Equals(_closed, msgImageKey._closed))
					return false;
				return true;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is MsgImageKey))
					return false;
				return Equals((MsgImageKey)obj);
			}

			public override int GetHashCode()
			{
				var result = _type.GetHashCode();
				result = 29 * result + _hasUnreaded.GetHashCode();
				result = 29 * result + _hasUnreadRepliesToMe.GetHashCode();
				result = 29 * result + _hasModeratorials.GetHashCode();
				result = 29 * result + _closed.GetHashCode();
				return result;
			}
		}
		#endregion

		#region ForumImageKey struct
		private struct ForumImageKey : IEquatable<ForumImageKey>
		{
			private readonly bool _hasUnread;
			private readonly bool _inTop;
			private readonly bool _hasUnreadRepliesToMe;

			public ForumImageKey(bool hasUnread, bool inTop, bool hasUnreadRepliesToMe)
			{
				_hasUnread = hasUnread;
				_inTop = inTop;
				_hasUnreadRepliesToMe = hasUnreadRepliesToMe;
			}

			public bool HasUnread => _hasUnread;

			public bool InTop => _inTop;

			public bool HasUnreadRepliesToMe => _hasUnreadRepliesToMe;

			public static bool operator !=(ForumImageKey forumImageKey1, ForumImageKey forumImageKey2)
			{
				return !forumImageKey1.Equals(forumImageKey2);
			}

			public static bool operator ==(ForumImageKey forumImageKey1, ForumImageKey forumImageKey2)
			{
				return forumImageKey1.Equals(forumImageKey2);
			}

			public bool Equals(ForumImageKey forumImageKey)
			{
				if (!Equals(_hasUnread, forumImageKey._hasUnread))
					return false;
				return Equals(_inTop, forumImageKey._inTop) && Equals(_hasUnreadRepliesToMe, forumImageKey._hasUnreadRepliesToMe);
			}

			public override bool Equals(object obj)
			{
				if (!(obj is ForumImageKey))
					return false;
				return Equals((ForumImageKey)obj);
			}

			public override int GetHashCode()
			{
				var result = _hasUnread.GetHashCode();
				result = 29 * result + _inTop.GetHashCode();
				result = 29 * result + _hasUnreadRepliesToMe.GetHashCode();
				return result;
			}
		}
		#endregion
	}
}
