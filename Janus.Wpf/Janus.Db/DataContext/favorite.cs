namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("favorite")]
	public partial class Favorite {
		public int id { get; set; }

		public int mid { get; set; }

		public int fid { get; set; }

		[StringLength(255)]
		public string comment { get; set; }

		[Column(TypeName = "ntext")]
		public string url { get; set; }
	}
}
