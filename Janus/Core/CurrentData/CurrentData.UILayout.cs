using System;
using System.Collections.Generic;
using System.Text;

namespace Rsdn.Janus
{
	// UI Layout
	partial class CurrentData
	{
		private int[] _forumViewerColumnWidths;

		public int[] ForumViewerColumnWidths
		{
			get { return _forumViewerColumnWidths; }
			set
			{
				_forumViewerColumnWidths = value;
				Save();
			}
		}
	}
}
