using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface ITextMacrosProvider
	{
		IEnumerable<ITextMacros> CreateTextMacroses();
	}
}