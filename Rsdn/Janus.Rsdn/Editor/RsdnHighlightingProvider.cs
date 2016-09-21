using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rsdn.Janus.Autocomplete
{
	[EditorHighlightingProvider]
	internal class RsdnHighlightingProvider : IHighlightingProvider
	{
		private static readonly Regex _tagRx = new Regex(
			@"\[/?(?<tag>[^\[\]=]+)=*[^\]]*\]",
			RegexOptions.Compiled);

		private static readonly Regex _smileRx = new Regex(
			@":facepalm:|:sarcasm:|:up:|:down:|:super:|:shuffle:" +
			@"|:crash:|:maniac:|:user:|:wow:|:beer:|:team:|:no:|" +
			@":nopont:|:xz:|(?<!:):-?\)\)\)|(?<!:):-?\)\)|(?<!:):-?\)|" +
			@"(?<!;|amp|gt|lt|quot);[-oO]?\)|(?<!:):-?\(|(?<!:):-[\\/]|:\?\?\?:");

		public IEnumerable<Highlighting> GetHighlightings(string line)
		{
			var tm = _tagRx.Match(line);
			while (tm.Success)
			{
				yield return
					new Highlighting(
						RsdnSyntaxInfo.KnownTags.ContainsKey(tm.Groups["tag"].Value)
							? HighlightType.KnownTags
							: HighlightType.UnknownTags,
						tm.Index,
						tm.Length);
				tm = tm.NextMatch();
			}

			var sm = _smileRx.Match(line);
			while (sm.Success)
			{
				yield return new Highlighting(HighlightType.Emoticons, sm.Index, sm.Length);
				sm = sm.NextMatch();
			}
		}
	}
}