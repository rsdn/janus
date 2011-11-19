using System;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация о провайдере элементов дерева навигации.
	/// </summary>
	public class NavigationTreeProviderInfo : ElementInfo
	{
		public NavigationTreeProviderInfo([NotNull] Type type) : base(type) { }
	}
}