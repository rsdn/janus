using System;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация о провайдере команд.
	/// </summary>
	public class CommandProviderInfo : ElementInfo
	{
		public CommandProviderInfo(Type type) : base(type) { }
	}
}