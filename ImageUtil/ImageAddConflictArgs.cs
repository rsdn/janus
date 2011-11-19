namespace ImageUtil
{
	public class ImageAddConflictArgs: ImageNameArgs
	{
		public ImageAddConflictArgs(string shortName, string fullName)
			: base(shortName, fullName)
		{ }

		public bool Cancel { get; set; }
	}
}