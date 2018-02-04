namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("favorites_folders")]
	public partial class FavoritesFolders {
		public int id { get; set; }

		[StringLength(100)]
		public string name { get; set; }

		public int pid { get; set; }

		[StringLength(255)]
		public string comment { get; set; }
	}
}
