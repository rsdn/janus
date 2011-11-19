using System;

namespace Rsdn.Janus
{
	public interface IDynamicNavigationTreeProvider : INavigationTreeProvider
	{
		IDisposable SubscribeNodesChanged(IServiceProvider serviceProvider, IObserver<EventArgs> observer);
	}
}