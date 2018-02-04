namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations.Schema;


	[Table("rate_outbox")]
	public partial class RateOutbox {
		public int id { get; set; }

		public int mid { get; set; }

		public int rate { get; set; }
	}
}
