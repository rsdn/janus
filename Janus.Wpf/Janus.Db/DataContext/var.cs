namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("var")]
	public partial class Var {
		[Key]
		[StringLength(24)]
		public string name { get; set; }

		[StringLength(128)]
		public string varvalue { get; set; }
	}
}
