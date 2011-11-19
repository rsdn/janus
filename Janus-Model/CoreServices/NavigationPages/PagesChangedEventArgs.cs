namespace Rsdn.Janus
{
	public class PagesChangedEventArgs
	{
		private readonly INavigationPage _oldPage;
		private readonly INavigationPage _newPage;
		private readonly PagesChangeType _changeType;

		public PagesChangedEventArgs(
			INavigationPage oldPage,
			INavigationPage newPage,
			PagesChangeType changeType)
		{
			_changeType = changeType;
			_newPage = newPage;
			_oldPage = oldPage;
		}

		public INavigationPage NewPage
		{
			get { return _newPage; }
		}

		public INavigationPage OldPage
		{
			get { return _oldPage; }
		}

		public PagesChangeType ChangeType
		{
			get { return _changeType; }
		}
	}
}