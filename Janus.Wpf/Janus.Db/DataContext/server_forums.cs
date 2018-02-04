namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("server_forums")]
	public partial class ServerForums {
		[StringLength(128)]
		public string descript { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int id { get; set; }

		[StringLength(64)]
		public string name { get; set; }

		public bool rated { get; set; }

		public bool intop { get; set; }

		public short ratelimit { get; set; }
	}
}
