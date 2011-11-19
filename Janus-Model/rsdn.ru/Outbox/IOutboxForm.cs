using System.Collections.Generic;

using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	public interface IOutboxForm
	{
		ICollection<ITreeNode> SelectedNodes { get; }
	}
}