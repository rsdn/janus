using System;

using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Помечает провайдер синхронизации.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (ISyncProvider))]
	public class SyncProviderAttribute : NamedElementAttribute
	{
		public SyncProviderAttribute(string name) : base(name)
		{}
	}
}