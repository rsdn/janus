using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface IOutboxRateCollection : ICollection<IOutboxRate>
	{
		void Add(int msgID, MessageRates rate);
	}
}