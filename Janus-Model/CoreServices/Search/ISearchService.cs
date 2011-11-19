using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface ISearchService
	{
		int AddMessagesToIndex(IEnumerable<MessageSearchInfo> messages);
	}
}