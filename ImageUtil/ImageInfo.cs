using System.Drawing;

namespace ImageUtil
{
	public class ImageInfo
	{
		private readonly string _fullName;
		private readonly string _name;
		private readonly Image _image;

		public ImageInfo(string fullName, string name, Image image)
		{
			_fullName = fullName;
			_name = name;
			_image = image;
		}

		public string FullName
		{
			get { return _fullName; }
		}

		public string ShortName
		{
			get { return _name; }
		}

		public Image Image
		{
			get { return _image; }
		}
	}
}