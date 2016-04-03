using System;

using CodeJam.Extensibility;

using JetBrains.Annotations;

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