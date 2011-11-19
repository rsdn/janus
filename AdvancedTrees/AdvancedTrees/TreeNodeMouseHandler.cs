using System.Drawing;
using System.Windows.Forms;

namespace AdvancedTrees
{
	public class TreeNodeMouseHandler<T> : ITreeNodeMouseHandler<T>
	{
		#region Implementation of ITreeNodeMouseHandler<T>

		public void MouseDown(TreePath<T> treePath, Rectangle nodeRect, MouseEventArgs mouseEventArgs)
		{
		}

		#endregion
	}
}