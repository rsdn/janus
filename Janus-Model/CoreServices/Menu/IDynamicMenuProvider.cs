using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface IDynamicMenuProvider : IMenuProvider
	{
		[NotNull]
		IObservable<EventArgs> MenuChanged { get; }
	}
}