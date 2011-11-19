using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface IContainerMenuItem : IMenuItem
	{
		[NotNull]
		string Name { get; }

		[NotNull]
		ICollection<IMenuGroup> Groups { get; }
	}
}