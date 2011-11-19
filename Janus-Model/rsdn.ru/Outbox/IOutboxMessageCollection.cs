using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface IOutboxMessageCollection : IEnumerable<IOutboxMessage>
	{
		int Count { get; }
		void Refresh();
	}
}