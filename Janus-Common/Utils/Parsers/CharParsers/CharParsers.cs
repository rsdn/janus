using System;

namespace Rsdn.Janus
{
	using CharResult = ParsingResult<CharInput, CharWithPosition>;
	using CharParser = Parser<CharInput, CharWithPosition>;

	public class CharParsers : Parsers
	{
		public static readonly CharParser AnyChar =
			input => new CharResult(input.Next(), new CharWithPosition(input.Current, input.Position));
		public static readonly CharParser WhiteSpace = Char(char.IsWhiteSpace);
		public static readonly CharParser Letter = Char(char.IsLetter);
		public static readonly CharParser LetterOrDigit = Char(char.IsLetterOrDigit);
		public static readonly CharParser Eof = Char('\0');

		public static CharParser WsChar(CharParser parser)
		{
			return from ws in ZeroOrMany(WhiteSpace)
				   from c in parser
				   select c;
		}

		public static CharParser Char(char ch)
		{
			return AnyChar.Where(cwp => cwp.Char == ch);
		}

		public static CharParser Char(Func<char, bool> predicate)
		{
			return AnyChar.Where(cwp => predicate(cwp.Char));
		}
	}
}