using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация о тег-лайнах.
	/// </summary>
	[Editor(typeof (TagLineInfoEditor), typeof (UITypeEditor))]
	public class TagLineInfos
	{

		private TagLineInfo[] _infos = new TagLineInfo[0];

		public static readonly TagLineInfos DefaultInfos =
			new TagLineInfos {Infos = new[] {TagLineInfo.DefaultInfo}};

		/// <summary>
		/// Коллекция тег-лайнов.
		/// </summary>
		[XmlElement("tag-line")]
		public TagLineInfo[] Infos
		{
			get { return _infos; }
			set { _infos = value; }
		}

		/// <summary>
		/// Преобразовать в строку.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(
				Infos.Length.GetDeclension(
					SR.Config.TagLine.Editor.Summary1,
					SR.Config.TagLine.Editor.Summary2,
					SR.Config.TagLine.Editor.Summary5),
				Infos.Length);
		}
	}
}