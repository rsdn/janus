namespace Rsdn.Janus
{
	public class DownloadTopicImpl : DownloadTopic
	{
		public DownloadTopicImpl(DownloadTopicCollection parent) : base(parent)
		{
		}

		public override int ID { get; set; }

		public override string Source { get; set; }

		public override int MessageID { get; set; }

		public override string Hint { get; set; }
	}
}