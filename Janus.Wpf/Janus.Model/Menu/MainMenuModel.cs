namespace Janus.Model.Menu {
	public class MainMenuModel : MenuModelBase {
		private ViewMenuModel _View = new ViewMenuModel();
		private ToolsMenuModel _Tools = new ToolsMenuModel();
		private FileMenuModel _File = new FileMenuModel();

		public FileMenuModel File { get { return _File; } }
		public ToolsMenuModel Tools { get { return _Tools; } }
		public ViewMenuModel View { get { return _View; } }
	}
}
