using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Команда меню или тулбара.
	/// </summary>
	public interface IMenuCommand : IMenuItemWithTextAndImage
	{
		string CommandName { get; }
		IDictionary<string, object> Parameters { get; }
	}
}