using System;

namespace Rsdn.Janus
{
	public interface ITextMacros
	{
		string DisplayName { get; }
		string MacrosText { get; }
		string GetResult(IServiceProvider serviceProvider);
	}
}