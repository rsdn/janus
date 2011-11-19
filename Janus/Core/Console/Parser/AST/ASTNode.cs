namespace Rsdn.Janus
{
	internal abstract class ASTNode
	{
		private readonly int _position;
		private readonly int _length;

		protected ASTNode(int position, int length)
		{
			_position = position;
			_length = length;
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