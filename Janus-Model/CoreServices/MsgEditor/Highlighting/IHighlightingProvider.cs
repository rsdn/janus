using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface IHighlightingProvider
	{
		IEnumerable<Highlighting> GetHighlightings(string line);
	}
}