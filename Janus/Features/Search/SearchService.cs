using System.Collections.Generic;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(ISearchService))]
	internal class SearchService : ISearchService
	{
		public int AddMessagesToIndex(IEnumerable<MessageSearchInfo> messages)
		{
			return SearchHelper.ProcessResponseMessages(messages);
		}
	}
}