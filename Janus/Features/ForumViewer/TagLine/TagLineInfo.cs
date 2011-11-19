using System.Xml.Serialization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация о тег-лайне.
	/// </summary>
	public class TagLineInfo
	{
		public const int AllForums = -1;

		private string _name = "";
		private string _format = "";
		private int[] _forums = new int[0];

		/// <summary>
		/// Имя тег-лайна.
		/// </summary>
		[XmlAttribute("name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Формат строки.
		/// </summary>
		[XmlText]
		public string Format
		{
			get { return _format; }
			set { _format = value; }
		}

		/// <summary>
		/// Список форумов, для которых теглайн использовать.
		/// </summary>
		[XmlElement("forum-id")]
		public int[] Forums
		{
			get { return _forums; }
			set { _forums = value; }
		}

		public static readonly TagLineInfo DefaultInfo =
			new TagLineInfo
			{
				Name = "Default",
				Format = "... << @@appname @@version @@release rev. @@revision>>",
				Forums = new[] {AllForums}
			};
	}
}
