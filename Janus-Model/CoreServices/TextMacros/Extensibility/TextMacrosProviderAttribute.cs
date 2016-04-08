using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут, помечающий источник состояния гаочек.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[BaseTypeRequired(typeof(ITextMacrosProvider))]
	[MeansImplicitUse]
	public class TextMacrosProviderAttribute : Attribute { }
}