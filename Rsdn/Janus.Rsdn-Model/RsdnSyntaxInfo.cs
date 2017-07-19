using System.Collections.Generic;

namespace Rsdn.Janus.Autocomplete
{
	/// <summary>
	/// Информация о синтаксисе RSDN сообщений.
	/// </summary>
	public class RsdnSyntaxInfo
	{
		public static readonly HashSet<string> KnownSmiles =
			new HashSet<string>
				{
					":", // not smile, but for prevention of automatic replacement ':' by ':)'
					":)",
					":))",
					":)))",
					":(",
					//";)", // this single smile, not started with ':', not used in autocomplete
					":-\\",
					":)",
					":up:",
					":down:",
					":super:",
					":shuffle:",
					":crash:",
					":maniac:",
					":user:",
					":wow:",
					":beer:",
					":team:",
					":no:",
					":nopont:",
					":xz:",
					":sarcasm:",
					":facepalm:",
					":???:?0" // scintilla use '?' as image marker, '?0' is never used
				};

		public static readonly IDictionary<string, bool> KnownTags =
			new Dictionary<string, bool>
				{
					{ "b", true },
					{ "i", true },
					{ "u", true },
					{ "s", true },
					{ "sub", true },
					{ "sup", true },
					{ "tt", true },
					{ "c#", true },
					{ "nemerle", true },
					{ "url", true },
					{ "img", true },
					{ "msil", true },
					{ "list", true },
					{ "*", false },
					{ "midl", true },
					{ "asm", true },
					{ "ccode", true },
					{ "code", true },
					{ "pascal", true },
					{ "vb", true },
					{ "sql", true },
					{ "java", true },
					{ "email", true },
					{ "msdn", true },
					{ "perl", true },
					{ "php", true },
					{ "erlang", true },
					{ "haskell", true },
					{ "lisp", true },
					{ "ml", true },
					{ "py", true },
					{ "rb", true },
					{ "prolog", true },
					{ "q", true },
					{ "hr", false },
					{ "article", false },
					{ "h1", true },
					{ "h2", true },
					{ "h3", true },
					{ "h4", true },
					{ "h5", true },
					{ "h6", true },
					{ "t", true },
					{ "th", true },
					{ "tr", true },
					{ "td", true },
					{ "xml", true }
				};
	}
}