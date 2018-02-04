namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("user")]
	public partial class User {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int uid { get; set; }

		[StringLength(120)]
		public string homepage { get; set; }

		[StringLength(255)]
		public string origin { get; set; }

		[StringLength(60)]
		public string publicemail { get; set; }

		[StringLength(80)]
		public string realname { get; set; }

		[StringLength(100)]
		public string spec { get; set; }

		public int? userclass { get; set; }

		[StringLength(60)]
		public string username { get; set; }

		[StringLength(100)]
		public string usernick { get; set; }

		[StringLength(100)]
		public string wherefrom { get; set; }
	}
}
