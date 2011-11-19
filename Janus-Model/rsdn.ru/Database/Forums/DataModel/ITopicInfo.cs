using System;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("topic_info")]
	public interface ITopicInfo
	{
		[MapField("mid")]
		int MessageID { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = false)]
		IForumMessage Message { get; }

		[MapField("gid")]
		int ForumID { get; }

		[MapField("this_rate")]
		int SelfRates { get; }

		[MapField("this_smile")]
		int SelfSmiles { get; }

		[MapField("this_agree")]
		int SelfAgrees { get; }

		[MapField("this_disagree")]
		int SelfDisagrees { get; }

		[MapField("this_mod_count")]
		int SelfModeratorials { get; }

		[MapField("answers_count")]
		int AnswersCount { get; }

		[MapField("answers_unread")]
		int AnswersUnread { get; }

		[MapField("answers_me_unread")]
		int AnswersToMeUnread { get; }

		[MapField("answers_rate")]
		int AnswersRates { get; }

		[MapField("answers_smile")]
		int AnswersSmiles { get; }

		[MapField("answers_agree")]
		int AnswersAgrees { get; }

		[MapField("answers_disagree")]
		int AnswersDisagrees { get; }

		[MapField("answers_mod_count")]
		int AnswersModeratorials { get; }

		[MapField("answers_marked")]
		int AnswersMarked { get; }

		[MapField("answers_last_update_date")]
		DateTime? LastUpdateDate { get; }
	}
}