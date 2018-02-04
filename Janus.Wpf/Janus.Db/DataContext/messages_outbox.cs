namespace Janus.Db.DataContext {
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("messages_outbox")]
	public partial class MessagesOutbox {
		public DateTime dte { get; set; }

		public int? gid { get; set; }

		public bool hold { get; set; }

		[Column(TypeName = "ntext")]
		public string message { get; set; }

		[Key]
		public int mid { get; set; }

		public int reply { get; set; }

		[StringLength(128)]
		public string subject { get; set; }

		[StringLength(128)]
		public string tagline { get; set; }

		[StringLength(1024)]
		public string tags { get; set; }
	}
}
