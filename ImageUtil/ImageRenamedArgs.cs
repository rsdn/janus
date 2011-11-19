using System;

namespace ImageUtil
{
	public class ImageRenamedArgs: EventArgs
	{
		private readonly string _prevFullName;
		private readonly string _newFullName;

		public ImageRenamedArgs(string prevFullName, string newFullName)
		{
			_prevFullName = prevFullName;
			_newFullName = newFullName;
		}

		public string PrevFullName
		{
			get { return _prevFullName; }
		}

		public string NewFullName
		{
			get { return _newFullName; }
		}
	}
}