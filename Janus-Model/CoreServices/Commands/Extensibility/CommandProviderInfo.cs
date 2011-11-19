using System;
using Rsdn.SmartApp;

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