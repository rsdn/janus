using Rsdn.TreeGrid;

namespace Rsdn.Janus.ObjectModel
{
	public interface IFeature : ITreeNode
	{
		string Description { get; }

		string Info { get; }

		int ImageIndex { get; }

		new IFeature this[int index] { get; }

		string Key { get; }
	}
}