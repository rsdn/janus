using System.Text.RegularExpressions;

namespace Rsdn.Janus
{
	public static class CommonRegexes
	{
		public static readonly Regex Name = new Regex(
			@"[\p{L}_]{1}[\p{L}_\p{Nd}]*",
			RegexOptions.Compiled);

		public static readonly Regex FullQualyfiedName = new Regex(
			@"([\p{L}_]{1}[\p{L}_\p{Nd}]*){1}(\.[\p{L}_]{1}[\p{L}_\p{Nd}]*)*",
			RegexOptions.Compiled);
	}
}