namespace Janus.Db.DataContext {
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("topic_info")]
	public partial class TopicInfo {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Column("MId")]
		[ForeignKey(nameof(Message))]
		public int Id { get; set; }

		[Column("answers_count")]
		public short? AnswersCount { get; set; }

		[Column("answers_unread")]
		public short? AnswersUnread { get; set; }

		[Column("answers_rate")]
		public short? AnswersRate { get; set; }

		[Column("answers_smile")]
		public short? AnswersSmile { get; set; }

		[Column("answers_agree")]
		public short? AnswersAgree { get; set; }

		[Column("answers_disagree")]
		public short? AnswersDisagree { get; set; }

		[Column("answers_me_unread")]
		public short? AnswersMeUnread { get; set; }

		[Column("answers_marked")]
		public short? AnswersMarked { get; set; }

		[Column("answers_last_update_date")]
		public DateTime? AnswersLastUpdateDate { get; set; }

		[Column("answers_mod_count")]
		public short? AnswersModCount { get; set; }

		[Column("this_rate")]
		public short? ThisRate { get; set; }

		[Column("this_smile")]
		public short? ThisSmile { get; set; }

		[Column("this_agree")]
		public short? ThisAgree { get; set; }

		[Column("this_disagree")]
		public short? ThisDisagree { get; set; }

		[Column("this_mod_count")]
		public short? ThisModCount { get; set; }

		[Column("gid")]
		public int? ForumId { get; set; }

		public virtual Message Message { get; set; }
	}
}
