using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Методы для упрощения копирования URL в клипбоард
	/// </summary>
	public static class ClipboardHelper
	{
		private const string _htmlTemplate =
			"Version:1.0\nStartHTML:0000000052\nEndHtml0000010000\n\n<html><body><a href='{0}'>{0}</a></body></html>";

		private const string _messageSubjectFormat = "JanusMessageSubject";

		public static string MessageSubjectFormat
		{
			get { return _messageSubjectFormat; }
		}

		public static void CopyUrl(string url)
		{
			CopyUrl(url, null);
		}

		public static void CopyUrl(string url, string name)
		{
			var dto = new DataObject(DataFormats.Html, string.Format(_htmlTemplate, url));
			dto.SetData(DataFormats.Text, url);
			if (!string.IsNullOrEmpty(name))
				dto.SetData(MessageSubjectFormat, name);
			Clipboard.SetDataObject(dto);
		}
	}
}