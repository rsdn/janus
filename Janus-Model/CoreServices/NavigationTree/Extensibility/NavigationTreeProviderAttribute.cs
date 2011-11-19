using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут, помечающий провайдера элементов дерева навигации.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[BaseTypeRequired(typeof(INavigationTreeProvider))]
	[MeansImplicitUse]
	public class NavigationTreeProviderAttribute : Attribute { }
}