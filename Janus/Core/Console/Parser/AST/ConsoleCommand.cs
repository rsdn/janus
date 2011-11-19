using System.Collections.Generic;
using System.Text;

namespace Rsdn.Janus
{
	internal class ConsoleCommand : ASTParentNode
	{
		private readonly ConsoleCommandName _commandName;
		private readonly ConsoleCommandArgument[] _arguments;

		public ConsoleCommand(
			int position,
			int length, 
			ConsoleCommandName name, 
			ConsoleCommandArgument[] arguments)
			: base(position, length)
		{
			_commandName = name;
			_arguments = arguments;
		}

		public ConsoleCommandName CommandName
		{
			get { return _commandName; }
		}

		public ConsoleCommandArgument[] Arguments
		{
			get { return _arguments; }
		}

		#region Overrides of ASTParentNode

		public override IEnumerator<ASTNode> GetEnumerator()
		{
			yield return CommandName;
			foreach (var argument in Arguments)
				yield return argument;
		}

		#endregion

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(CommandName);
			foreach (var argument in Arguments)
			{
				sb.Append(' ');
				sb.Append(argument.ToString());
			}
			return sb.ToString();
		}
	}
}