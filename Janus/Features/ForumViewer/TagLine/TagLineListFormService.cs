using System;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	internal class TagLineListFormService : ITagLineListFormService
	{
		private readonly TagLineListForm _form;

		public TagLineListFormService(TagLineListForm form)
		{
			if (form == null)
				throw new ArgumentNullException(nameof(form));

			_form = form;
		}

		#region ITagLineListFormService Members

		public event SelectedTagLinesChangedEventHandler SelectedTagLinesChanged
		{
			add { _form.SelectedTagLinesChanged += value; }
			remove { _form.SelectedTagLinesChanged -= value; }
		}

		public IEnumerable<TagLineInfo> SelectedTagLines => _form.SelectedTagLines;

		public ICollection<TagLineInfo> TagLines => _form.TagLines;
		#endregion
	}
}