using System;

namespace ImageUtil
{
	public class ImageNameArgs: EventArgs
	{
		private readonly string _shortName;
		private readonly string _fullName;

		public ImageNameArgs(string shortName, string fullName)
		{
			_shortName = shortName;
			_fullName = fullName;
		}

		public string ShortName
		{
			get { return _shortName; }
		}

		public string FullName
		{
			get { return _fullName; }
		}
	}
}