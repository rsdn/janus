using System;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация об обработчике команды.
	/// </summary>
	public class CommandTargetInfo : ElementInfo
	{
		public CommandTargetInfo(Type type) : base(type) { }
	}
}