using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("download_topics")]
	public interface IDownloadTopic
	{
		[Column] int ID { get; }
		[Column] string Source { get; }
		[Column] int MessageID { get; }
		[Column] string Hint { get; }
	}
}