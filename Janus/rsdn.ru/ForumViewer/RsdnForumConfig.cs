namespace Rsdn.Janus
{
	internal class RsdnForumConfig : IRsdnForumConfig
	{
		public bool ShowFullForumNames
		{
			get { return Config.Instance.ForumDisplayConfig.ShowFullForumNames; }
		}
	}
}