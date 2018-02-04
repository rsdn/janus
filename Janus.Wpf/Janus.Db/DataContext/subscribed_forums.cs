namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("subscribed_forums")]
	public partial class SubscribedForums {
		[StringLength(128)]
		public string descript { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int id { get; set; }

		public int lastsync { get; set; }

		[StringLength(64)]
		public string name { get; set; }

		public int? urcount { get; set; }

		public bool issync { get; set; }

		public int? priority { get; set; }
	}
}
