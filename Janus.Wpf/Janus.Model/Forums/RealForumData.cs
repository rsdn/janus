namespace Janus.Model.Forums {
	public class RealForumData : ForumTreeData {
		private int _MessageCount;
		private int _UnreadCount;
		private int _AnswersToMeCount;
		private bool _IsSubscribed;

		public override string ImageKey {
			get => "Forum";
			set { }
		}

		public int MessageCount {
			get {
				return _MessageCount;
			}
			set {
				_MessageCount = value;
				OnPropertyChanged();
			}
		}

		public int UnreadCount {
			get {
				return _UnreadCount;
			}
			set {
				_UnreadCount = value;
				OnPropertyChanged();
			}
		}

		public int AnswersToMeCount {
			get {
				return _AnswersToMeCount;
			}
			set {
				_AnswersToMeCount = value;
				OnPropertyChanged();
			}
		}

		public bool IsSubscribed {
			get { return _IsSubscribed; }
			set {
				if (_IsSubscribed == value) {
					return;
				}
				_IsSubscribed = value;
				OnPropertyChanged();
			}
		}

		public int Id { get; set; }
	}
}
