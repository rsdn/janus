using System.Collections.ObjectModel;

namespace Janus.Model.Forums {
	public class OutboxData : ForumTreeData {

		public ObservableCollection<MessageData> Messages { get; } = new ObservableCollection<MessageData>();
		public ObservableCollection<MarkData> Marks { get; } = new ObservableCollection<MarkData>();

		public ObservableCollection<ReloadThemeData> ThemesReload { get; } = new ObservableCollection<ReloadThemeData>();

		public override string Title { get => "Outbox"; set { } }

		public override string ImageKey {
			get => "Outbox";
			set { }
		}

	}
}