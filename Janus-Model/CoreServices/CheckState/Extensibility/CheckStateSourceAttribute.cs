using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут, помечающий источник состояния гаочек.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	[BaseTypeRequired(typeof(ICheckStateSource))]
	[MeansImplicitUse]
	public class CheckStateSourceAttribute : Attribute { }
}