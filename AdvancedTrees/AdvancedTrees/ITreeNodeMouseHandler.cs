using System.Drawing;
using System.Windows.Forms;

namespace AdvancedTrees
{
	public interface ITreeNodeMouseHandler<T>
	{
		//void MouseClick();
		//void MouseDoubleClick();
		void MouseDown(TreePath<T> treePath, Rectangle nodeRect, MouseEventArgs mouseEventArgs);
		//void MouseUp();
		//void MouseEnter();
		//void MouseLeave();
		//void MouseMove();
	}
}