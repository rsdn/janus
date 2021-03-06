﻿using System.Collections.Generic;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("message_tags")]
	public interface IMessageTag
	{
		[Column("message_id")]
		int MessageID { get; }

		[Column("tag_id")]
		int TagID { get; }

		[Association(ThisKey = "TagID", OtherKey = "ID")]
		ITag Tag { get; }
	}
}