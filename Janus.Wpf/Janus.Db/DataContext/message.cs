namespace Janus.Db.DataContext {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("message")]
	public partial class Message {
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Message() {
			Tags = new HashSet<Tag>();
		}

		[Column("DTE")]
		public DateTime Date { get; set; }

		[Column("GId")]
		public int ForumId { get; set; }

		public bool IsMarked { get; set; }

		public byte IsRead { get; set; }

		[Column("message", TypeName = "ntext")]
		public string MessageText { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Column("MId")]
		public int Id { get; set; }

		[Column("PId")]
		public int ParentId { get; set; }

		[StringLength(128)]
		public string Subject { get; set; }

		[Column("tid")]
		[ForeignKey(nameof(TopicInfo))]
		public int TopicId { get; set; }

		public int? UClass { get; set; }

		public int UId { get; set; }

		[StringLength(50)]
		public string UserNick { get; set; }

		[Column("article_id")]
		public int? ArticleId { get; set; }

		public bool ReadReplies { get; set; }

		[StringLength(160)]
		public string Name { get; set; }

		public DateTime? LastModerated { get; set; }

		public bool Closed { get; set; }

		public virtual TopicInfo TopicInfo { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Tag> Tags { get; set; }
	}
}
