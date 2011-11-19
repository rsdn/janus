using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	[XmlRoot("Message")]
	public class XmlMessage
	{
		public class MessageFormattingOptions
		{
			public bool ShowHeader { get; set; }
			public bool ShowRateFrame { get; set; }
		}

		public class MessageUserInfo
		{
			public int ID { get; set; }
			public int UserClass { get; set; }
			public string DisplayName { get; set; }
		}

		public class MessageDate
		{
			public bool IsOutdate { get; set; }
			public int DayOfWeek { get; set; }
			public string Value { get; set; }
		}

		public class MessageRate
		{
			public string Summary { get; set; }

			private List<MessageRateItem> _rateList = new List<MessageRateItem>();
			[XmlArray("RateList")]
			[XmlArrayItem("RateItem")]
			public List<MessageRateItem> List
			{
				get { return _rateList; }
				set { _rateList = value; }
			}
		}

		public class MessageRateItem
		{
			public bool ForumInTop { get; set; }
			public int Type { get; set; }
			public string Value { get; set; }

			private MessageUserInfo _author = new MessageUserInfo();
			public MessageUserInfo Author
			{
				get { return _author; }
				set { _author = value; }
			}
		}

		private MessageFormattingOptions _formattingOptions = new MessageFormattingOptions();
		public MessageFormattingOptions FormattingOptions
		{
			get { return _formattingOptions; }
			set { _formattingOptions = value; }
		}

		private MessageRate _rate = new MessageRate();
		public MessageRate Rate
		{
			get { return _rate; }
			set { _rate = value; }
		}

		private MessageUserInfo _author = new MessageUserInfo();
		public MessageUserInfo Author
		{
			get { return _author; }
			set { _author = value; }
		}

		private MessageDate _date = new MessageDate();
		public MessageDate Date
		{
			get { return _date; }
			set { _date = value; }
		}

		public int ID { get; set; }
		public string Name { get; set; }
		public bool IsUnread { get; set; }
		public bool IsMarked { get; set; }
		public int ArticleID { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
		public string Origin { get; set; }
	}
}
