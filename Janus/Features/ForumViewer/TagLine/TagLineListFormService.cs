using System;
using System.Collections.Generic;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal class TagLineListFormService : ITagLineListFormService
	{
		private readonly TagLineListForm _form;

		public TagLineListFormService(TagLineListForm form)
		{
			if (form == null)
				throw new ArgumentNullException("form");

			_form = form;
		}

		#region ITagLineListFormService Members

		public event SelectedTagLinesChangedEventHandler SelectedTagLinesChanged
		{
			add { _form.SelectedTagLinesChanged += value; }
			remove { _form.SelectedTagLinesChanged -= value; }
		}

		public IEnumerable<TagLineInfo> SelectedTagLines
		{
			get { return _form.SelectedTagLines; }
		}

		public ICollection<TagLineInfo> TagLines
		{
			get { return _form.TagLines; }
		}

		#endregion
	}
}