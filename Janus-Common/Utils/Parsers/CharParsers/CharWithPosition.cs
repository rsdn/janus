namespace Rsdn.Janus
{
	public struct CharWithPosition
	{
		private readonly char _char;
		private readonly int _position;

		public CharWithPosition(char chr, int position)
		{
			_char = chr;
			_position = position;
		}

		public char Char
		{
			get { return _char; }
		}

		public int Position
		{
			get { return _position; }
		}
	}
}