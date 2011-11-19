using System.Collections.Generic;

namespace Rsdn.Janus
{
	public interface ITagLineListFormService
	{
		event SelectedTagLinesChangedEventHandler SelectedTagLinesChanged;
		IEnumerable<TagLineInfo> SelectedTagLines { get; }
		ICollection<TagLineInfo> TagLines { get; }
	}

	public delegate void SelectedTagLinesChangedEventHandler(object sender);
}