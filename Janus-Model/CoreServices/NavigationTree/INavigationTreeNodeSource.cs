using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface INavigationTreeNodeSource
	{
		[NotNull]
		string Name { get; }
	}
}