using BLToolkit.DataAccess;

namespace Rsdn.Janus.DataModel
{
	[TableName("download_topics")]
	public interface IDownloadTopic
	{
		int ID { get; }
		string Source { get; }
		int MessageID { get; }
		string Hint { get; }
	}
}