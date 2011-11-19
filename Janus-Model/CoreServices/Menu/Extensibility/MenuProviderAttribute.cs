using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут, помечающий провайдера меню.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[BaseTypeRequired(typeof(IMenuProvider))]
	[MeansImplicitUse]
	public class MenuProviderAttribute : Attribute { }
}