using System.Drawing.Imaging;
using System.Net.Mime;

namespace Rsdn.Janus.Framework.Imaging
{
	public class ImageFormatInfo
	{
		public static readonly ImageFormatInfo Bmp = new ImageFormatInfo("bmp", "image/bmp");
		public static readonly ImageFormatInfo Emf = new ImageFormatInfo("emf", "application/x-msmetafile");
		public static readonly ImageFormatInfo Exif = new ImageFormatInfo("exif", "image/x-exif");

		public static readonly ImageFormatInfo[] Formats =
			new[]
			{
				Bmp, Emf, Wmf, Exif, Gif, Icon, Jpeg, Png, Tiff
			};

		public static readonly ImageFormatInfo Gif = new ImageFormatInfo("gif", MediaTypeNames.Image.Gif);
		public static readonly ImageFormatInfo Icon = new ImageFormatInfo("ico", "image/x-icon");

		public static readonly ImageFormatInfo Jpeg = new ImageFormatInfo("jpeg",
			MediaTypeNames.Image.Jpeg);

		public static readonly ImageFormatInfo Png = new ImageFormatInfo("png", "image/x-png");

		public static readonly ImageFormatInfo Tiff = new ImageFormatInfo("tiff",
			MediaTypeNames.Image.Tiff);

		public static readonly ImageFormatInfo Wmf = new ImageFormatInfo("wmf", "application/x-msmetafile");
		private readonly string _extension;
		private readonly string _mimeType;

		private ImageFormatInfo(string extension, string mimeType)
		{
			_extension = extension;
			_mimeType = mimeType;
		}

		public string Extension
		{
			get { return _extension; }
		}

		public string MimeType
		{
			get { return _mimeType; }
		}

		public static ImageFormatInfo FromImageFormat(ImageFormat format)
		{
			if (format.Equals(ImageFormat.Bmp) || format.Equals(ImageFormat.MemoryBmp))
				return Bmp;
			if (format.Equals(ImageFormat.Emf))
				return Emf;
			if (format.Equals(ImageFormat.Wmf))
				return Wmf;
			if (format.Equals(ImageFormat.Exif))
				return Exif;
			if (format.Equals(ImageFormat.Gif))
				return Gif;
			if (format.Equals(ImageFormat.Icon))
				return Icon;
			if (format.Equals(ImageFormat.Jpeg))
				return Jpeg;
			if (format.Equals(ImageFormat.Png))
				return Png;
			if (format.Equals(ImageFormat.Tiff))
				return Tiff;
			return Bmp;
		}
	}
}