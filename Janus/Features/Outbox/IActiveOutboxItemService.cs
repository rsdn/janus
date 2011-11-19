using System;
using System.Collections.Generic;

using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	public interface IActiveOutboxItemService
	{
		List<ITreeNode> ActiveOutboxItems { get; }
		event EventHandler ActiveOutboxItemsChanged;
	}
}