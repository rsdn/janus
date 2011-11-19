namespace Rsdn.Janus
{
	public class StringInput : CharInput
	{
		private readonly string _str;
		private readonly int _position;

		private StringInput(string str, int position)
		{
			_str = str;
			_position = position;
		}

		public StringInput(string str) : this(str, 0) { }

		public override char Current
		{
			get { return _position < _str.Length ? _str[_position] : '\0'; }
		}

		public override int Position
		{
			get { return _position; }
		}

		public override CharInput Next()
		{
			return new StringInput(_str, _position + 1);
		}
	}
}