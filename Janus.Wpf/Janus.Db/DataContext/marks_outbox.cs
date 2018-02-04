namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("marks_outbox")]
	public partial class MarksOutbox {
		public int id { get; set; }

		public int mark { get; set; }

		public int mid { get; set; }
	}
}
