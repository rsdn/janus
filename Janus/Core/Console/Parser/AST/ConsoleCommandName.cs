namespace Rsdn.Janus
{
	internal class ConsoleCommandName : ASTNode
	{
		private readonly string _name;

		public ConsoleCommandName(int position, int length, string name)
			: base(position, length)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}

		public override string ToString()
		{
			return Name;
		}
	}
}