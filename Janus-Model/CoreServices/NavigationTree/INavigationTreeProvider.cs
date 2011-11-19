using System;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface INavigationTreeProvider
	{
		IEnumerable<INavigationTreeNodeSource> CreateNodes(IServiceProvider serviceProvider);
	}
}