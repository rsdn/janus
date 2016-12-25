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

			[XmlArray("RateList")]
			[XmlArrayItem("RateItem")]
			public List<MessageRateItem> List { get; set; } = new List<MessageRateItem>();
		}

		public class MessageRateItem
		{
			public bool ForumInTop { get; set; }
			public int Type { get; set; }
			public string Value { get; set; }

			public MessageUserInfo Author { get; set; } = new MessageUserInfo();
		}

		public MessageFormattingOptions FormattingOptions { get; set; } = new MessageFormattingOptions();

		public MessageRate Rate { get; set; } = new MessageRate();

		public MessageUserInfo Author { get; set; } = new MessageUserInfo();

		public MessageDate Date { get; set; } = new MessageDate();

		public int ID { get; set; }
		public string Name { get; set; }
		public bool IsUnread { get; set; }
		public bool IsMarked { get; set; }
		public int ArticleID { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
		public string Origin { get; set; }
		public int ViolationPenaltyType { get; set; }
		public string ViolationReason { get; set; }
		public string[] Tags { get; set; }
	}
}
