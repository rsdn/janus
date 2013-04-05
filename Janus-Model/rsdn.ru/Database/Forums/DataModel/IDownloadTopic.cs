using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("download_topics")]
	public interface IDownloadTopic
	{
		int ID { get; }
		string Source { get; }
		int MessageID { get; }
		string Hint { get; }
	}
}