using System.Collections.Generic;

namespace Rsdn.Janus
{
	/// <summary>
	/// Элемент меню с чекбоксом.
	/// </summary>
	public interface IMenuCheckCommand : IMenuItemWithTextAndImage
	{
		string CheckStateName { get; }
		string CheckCommandName { get; }
		IDictionary<string, object> CheckCommandParameters { get; }
		string UncheckCommandName { get; }
		IDictionary<string, object> UncheckCommandParameters { get; }
	}
}