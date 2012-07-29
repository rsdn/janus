namespace Rsdn.Janus
{
	public class Highlighting
	{
		private readonly HighlightType _type;
		private readonly int _position;
		private readonly int _length;

		public Highlighting(HighlightType type, int position, int length)
		{
			_type = type;
			_position = position;
			_length = length;
		}

		public HighlightType Type
		{
			get { return _type; }
		}

		public int Position
		{
			get { return _position; }
		}

		public int Length
		{
			get { return _length; }
		}
	}
}