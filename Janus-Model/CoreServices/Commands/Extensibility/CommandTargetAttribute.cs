using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут, помечающий обработчик команд.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	[MeansImplicitUse]
	[BaseTypeRequired(typeof(ICommandTarget))]
	public class CommandTargetAttribute : Attribute { }
}