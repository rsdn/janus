using System.Collections.Generic;

using BLToolkit.Data.Linq;

namespace Rsdn.Janus
{
	public interface IDownloadTopicCollection : IEnumerable<IDownloadTopic>
	{
		int Count { get; }
		bool Add(string source, int messageId, string hint);
		void Clear(IDataContext db);
	}
}