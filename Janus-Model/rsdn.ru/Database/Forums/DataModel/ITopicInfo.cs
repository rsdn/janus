using System;

using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("topic_info")]
	public interface ITopicInfo
	{
		[Column("mid")]
		int MessageID { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = false)]
		IForumMessage Message { get; }

		[Column("gid")]
		int ForumID { get; }

		[Column("this_rate")]
		int SelfRates { get; }

		[Column("this_smile")]
		int SelfSmiles { get; }

		[Column("this_agree")]
		int SelfAgrees { get; }

		[Column("this_disagree")]
		int SelfDisagrees { get; }

		[Column("this_mod_count")]
		int SelfModeratorials { get; }

		[Column("answers_count")]
		int AnswersCount { get; }

		[Column("answers_unread")]
		int AnswersUnread { get; }

		[Column("answers_me_unread")]
		int AnswersToMeUnread { get; }

		[Column("answers_rate")]
		int AnswersRates { get; }

		[Column("answers_smile")]
		int AnswersSmiles { get; }

		[Column("answers_agree")]
		int AnswersAgrees { get; }

		[Column("answers_disagree")]
		int AnswersDisagrees { get; }

		[Column("answers_mod_count")]
		int AnswersModeratorials { get; }

		[Column("answers_marked")]
		int AnswersMarked { get; }

		[Column("answers_last_update_date")]
		DateTime? LastUpdateDate { get; }
	}
}