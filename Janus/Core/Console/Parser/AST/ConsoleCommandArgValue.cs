namespace Rsdn.Janus
{
	internal class ConsoleCommandArgValue : ASTNode
	{
		private readonly string _value;

		public ConsoleCommandArgValue(int position, int length, string value)
			: base(position, length)
		{
			_value = value;
		}

		public string Value
		{
			get { return _value; }
		}

		public override string ToString()
		{
			return '"'+ Value + '"';
		}
	}
}