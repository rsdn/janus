using System.Linq;
using System.Text;

namespace Rsdn.Janus
{
	internal class ConsoleParser
	{
		public static ConsoleCommand Parse(string str)
		{
			var parser = ConsoleParsers.Command(new StringInput(str));
			return parser != null ? parser.Value : null;
		}
	}

	internal class ConsoleParsers : CharParsers
	{
		public static readonly Parser<CharInput, Token> Id =
			from cwp in WsChar(Letter.Or(Char('_')))
			from cwps in ZeroOrMany(LetterOrDigit.Or(Char('_')))
			let text = cwps.Aggregate(
				new StringBuilder().Append(cwp.Char),
				(acc, subCwp) => acc.Append(subCwp.Char),
				sb => sb.ToString())
			select new Token(text, cwp.Position, text.Length);

		public static readonly Parser<CharInput, Token> FullQualyfiedId =
			from id in Id
			from isd in ZeroOrMany(Char('.').And(Id, (dot, subId) => subId.Text))
			let text = isd.Aggregate(
				new StringBuilder().Append(id.Text),
				(acc, subId) => acc.Append('.').Append(subId),
				sb => sb.ToString())
			select new Token(text, id.Position, text.Length);

		public static readonly Parser<CharInput, Token> SimpleArgValue =
			from ws in ZeroOrMany(WhiteSpace)
			from chars in OneOrMany(Char(c => !char.IsWhiteSpace(c) && c != '\0'))
			select new Token(
				chars.Select(cwp => cwp.Char).JoinToString(),
				chars.First().Position,
				chars.Length);

		public static readonly Parser<CharInput, Token> QuotedArgValue =
			from lb in WsChar(Char('"'))
			from chars in ZeroOrMany(Char(c => c != '"' && c != '\0'))
			from rb in Char('"')
			select new Token(
				chars.Select(cwp => cwp.Char).JoinToString(),
				lb.Position,
				chars.Length + 2);

		public static readonly Parser<CharInput, ConsoleCommandName> CommandName =
			FullQualyfiedId.Select(
				token => new ConsoleCommandName(token.Position, token.Length, token.Text));

		public static readonly Parser<CharInput, ConsoleCommandArgName> ArgName =
			Id.Select(token => new ConsoleCommandArgName(token.Position, token.Length, token.Text));

		public static readonly Parser<CharInput, ConsoleCommandArgValue> ArgValue =
			QuotedArgValue.Or(SimpleArgValue)
				.Select(token => new ConsoleCommandArgValue(token.Position, token.Length, token.Text));

		public static readonly Parser<CharInput, ConsoleCommandArgument> Arg =
			from argName in ArgName
			from spl in WsChar(Char('='))
			from argValue in ArgValue
			select new ConsoleCommandArgument(
				argName.Position,
				argValue.Position + argValue.Length - argName.Position + 1,
				argName,
				argValue);

		public static readonly Parser<CharInput, ConsoleCommand> Command =
			from name in CommandName
			from args in ZeroOrMany(Arg)
			from eof in WsChar(Eof)
			select new ConsoleCommand(name.Position, eof.Position - name.Position, name, args);
	}

	public struct Token
	{
		private readonly string _text;
		private readonly int _position;
		private readonly int _length;

		public Token(string text, int position, int length)
		{
			_text = text;
			_length = length;
			_position = position;
		}

		public int Length
		{
			get { return _length; }
		}

		public string Text
		{
			get { return _text; }
		}

		public int Position
		{
			get { return _position; }
		}
	}
}