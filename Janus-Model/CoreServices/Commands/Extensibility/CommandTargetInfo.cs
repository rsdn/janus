using System;

using Rsdn.SmartApp;

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