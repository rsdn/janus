using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface ITextMacrosService
	{
		ICollection<ITextMacros> TextMacroses { get; }
		ITextMacros GetTextMacros(string macrosText);
	}
}