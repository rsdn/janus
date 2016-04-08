namespace Rsdn.Janus
{
	public class PagesChangedEventArgs
	{
		public PagesChangedEventArgs(
			INavigationPage oldPage,
			INavigationPage newPage,
			PagesChangeType changeType)
		{
			ChangeType = changeType;
			NewPage = newPage;
			OldPage = oldPage;
		}

		public INavigationPage NewPage { get; }

		public INavigationPage OldPage { get; }

		public PagesChangeType ChangeType { get; }
	}
}