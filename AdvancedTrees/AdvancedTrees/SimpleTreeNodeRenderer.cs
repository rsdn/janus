using System;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

namespace AdvancedTrees
{
	public class SimpleTreeNodeRenderer<T> : ITreeNodeRenderer<T>
	{
		private readonly Font _font;
		private readonly Color _foreColor;
		private readonly Color _backColor;

		public SimpleTreeNodeRenderer([NotNull] Font font, Color foreColor, Color backColor)
		{
			if (font == null)
				throw new ArgumentNullException("font");

			_font = font;
			_backColor = backColor;
			_foreColor = foreColor;
		}

		#region Implementation of ITreeNodeRenderer<T>

		public int NodeHeight
		{
			get { return _font.Height; }
		}

		public void DrawNode(
			Graphics graphics,
			Rectangle nodeRect,
			T node,
			bool isSelected,
			bool isLeaf,
			bool isExpanded)
		{
			graphics.FillRectangle(isSelected ? SystemBrushes.Highlight : new SolidBrush(_backColor), nodeRect);
			TextRenderer.DrawText(
				graphics,
				node.ToString(),
				_font,
				nodeRect,
				isSelected ? SystemColors.HighlightText : _foreColor,
				TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
		}

		#endregion
	}
}