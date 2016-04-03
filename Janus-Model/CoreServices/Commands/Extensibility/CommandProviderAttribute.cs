using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут, помечающий провайдер команд.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[BaseTypeRequired(typeof(ICommandProvider))]
	[MeansImplicitUse]
	public class CommandProviderAttribute : Attribute { }
}