using System.Collections.Generic;

namespace Rsdn.Janus
{
	internal class ConsoleCommandArgument : ASTParentNode
	{
		private readonly ConsoleCommandArgName _name;
		private readonly ConsoleCommandArgValue _value;

		public ConsoleCommandArgument(
			int position,
			int length,
			ConsoleCommandArgName name,
			ConsoleCommandArgValue value)
			: base(position, length)
		{
			_name = name;
			_value = value;
		}

		public ConsoleCommandArgValue Value
		{
			get { return _value; }
		}

		public ConsoleCommandArgName Name
		{
			get { return _name; }
		}

		public override IEnumerator<ASTNode> GetEnumerator()
		{
			yield return Name;
			yield return Value;
		}

		public override string ToString()
		{
			return string.Format("{0} = {1}", Name, Value);
		}
	}
}