using System.Drawing;

namespace AdvancedTrees
{
	public interface ITreeNodeRenderer<T>
	{
		int NodeHeight { get; }

		void DrawNode(
			Graphics graphics,
			Rectangle nodeRect,
			T node,
			bool isSelected,
			bool isLeaf,
			bool isExpanded);
	}
}